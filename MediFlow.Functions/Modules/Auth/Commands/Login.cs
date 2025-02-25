using Mediator;
using MediFlow.Functions.Data.Services.PasswordHasher;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Functions.Modules.Auth.Services.JwtProvider;
using MediFlow.Functions.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Functions.Modules.Auth.Commands;
using IResult = Result<LoginResponse, AuthErrors>;

public record LoginCommand(string Email, string Password) : ICommand<IResult>;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IEmployeeRepository emploeeRepository,
    IStructureRepository structureRepository,
    ISessionRepository sessionRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IRefreshCookieAccessor refreshCookieAccessor,
    IDeviceCookieAccessor deviceCookieAccessor) : ICommandHandler<LoginCommand, IResult>
{
    public async ValueTask<IResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetOneAsync(command.Email);
        if (user is null)
        {
            return AuthErrors.UserByEmailNotFound;
        }

        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);

        if (!passwordHasher.Veriffy(command.Password, user.PasswordHash))
        {
            return AuthErrors.PasswordDoesntMatch;
        }

        if (!(user.Role == UserRoles.Admin || user.Role == UserRoles.Manager || hasDeviceKey))
        {
            return AuthErrors.NoDeviceKey;
        }

        user.RefreshToken = Guid.NewGuid();


        var accessToken = string.Empty;
        var expirationDate = default(DateTime);
        var structureId = default(StructureId);
        var employeeId = default(EmployeeId);
        var employeeRole = EmployeeRoles.NotSpecified;


        if (user.Role == UserRoles.Employee)
        {
            var structureExists = await structureRepository.ExistsAsync(deviceKey);
            if (!structureExists)
            {
                await deviceCookieAccessor.DeleteCookieAsync();
                return AuthErrors.StructurByDeviceKeyNotFound;
            }

            var employee = await emploeeRepository.GetOneAsync(user.Id, deviceKey);

            if (employee is null)
            {
                return AuthErrors.UserNotEnrolledInStructure;
            }

            structureId = employee.StructureId;
            employeeId = employee.Id;
            employeeRole = employee.Role;

            accessToken = jwtProvider.GenerateAcessToken(
                user.Id,
                user.Role,
                structureId,
                employeeRole,
                employeeId,
                out expirationDate);
        }
        else if (user.Role == UserRoles.Manager || user.Role == UserRoles.Admin)
        {
            if (hasDeviceKey)
            {
                var structure = await structureRepository.GetOneAsync(deviceKey);
                if (structure is null)
                {
                    await deviceCookieAccessor.DeleteCookieAsync();
                }
                else
                {
                    structureId = structure.Id;
                }
            }
            if (structureId == default && user.Role == UserRoles.Manager)
            {
                var lastSession = await sessionRepository.GetOneAsync(user.Id);
                if (lastSession is not null)
                {
                    structureId = lastSession.StructureId;
                }
            }

            accessToken = structureId.IsEmpty()
                ? jwtProvider.GenerateAcessToken(user.Id, user.Role, out expirationDate)
                : jwtProvider.GenerateAcessToken(user.Id, user.Role, structureId, out expirationDate);
        }

        await refreshCookieAccessor.SetCookieAsync(user.RefreshToken);
        await unitOfWork.SaveChangesAsync();

        return new LoginResponse()
        {
            AccessToken = accessToken,
            ExpirationDate = expirationDate,
            SessionData = new()
            {
                UserId = user.Id,
                UserRole = user.Role,
                StructureId = structureId,
                EmployeeId = employeeId,
                EmployeeRole = employeeRole,
            }
        };
    }
}
