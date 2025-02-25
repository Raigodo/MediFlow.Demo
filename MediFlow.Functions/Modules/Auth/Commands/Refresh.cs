using Mediator;
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
using Result = Result<RefreshResponse, AuthErrors>;

public record RefreshCommand(UserId CurrentUserId) : ICommand<Result>;

public class RefreshCommandHandler(
    IUserRepository userRepository,
    IEmployeeRepository emploeeRepository,
    IStructureRepository structureRepository,
    ISessionRepository sessionRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider,
    IRefreshCookieAccessor refreshCookieAccessor,
    IDeviceCookieAccessor deviceCookieAccessor) : ICommandHandler<RefreshCommand, Result>
{
    public async ValueTask<Result> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);

        var user = await userRepository.GetOneAsync(command.CurrentUserId);
        if (user is null)
        {
            return AuthErrors.CurrentUserNotFound;
        }

        if (!refreshCookieAccessor.TryGetRefreshToken(out var refreshToken))
        {
            if (user.RefreshToken != refreshToken)
            {
                await refreshCookieAccessor.DeleteCookieAsync();
            }
            return AuthErrors.NoRefreshToken;
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
                return AuthErrors.NoDeviceKey;
            }

            var employee = await emploeeRepository.GetOneAsync(user.Id, deviceKey);

            if (employee is null)
            {
                return AuthErrors.StructurByDeviceKeyNotFound;
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

        return new RefreshResponse()
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
