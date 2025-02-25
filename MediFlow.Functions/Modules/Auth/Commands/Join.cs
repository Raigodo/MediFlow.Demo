using Mediator;
using MediFlow.Functions.Data.Services.PasswordHasher;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Functions.Modules.Auth.Services.JwtProvider;
using MediFlow.Functions.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Functions.Modules.Auth.Commands;
using Result = Result<JoinResponse, AuthErrors>;

public record JoinCommand(string Email, string Password, InvitationId InvitationId) : ICommand<Result>;

public class JoinCommandHandler(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IStructureRepository structureRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ISessionRepository sessionRepository,
    IJwtProvider jwtProvider,
    IRefreshCookieAccessor refreshCookieAccessor,
    IDeviceCookieAccessor deviceCookieAccessor) : ICommandHandler<JoinCommand, Result>
{
    public async ValueTask<Result> Handle(JoinCommand command, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepository.GetOneAsync(command.InvitationId);
        if (invitation is null)
        {
            return AuthErrors.InvitationNotFound;
        }

        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);

        var user = await userRepository.GetOneAsync(command.Email);
        if (user is null)
        {
            return AuthErrors.UserByEmailNotFound;
        }

        if (!passwordHasher.Veriffy(command.Password, user.PasswordHash))
        {
            return AuthErrors.PasswordDoesntMatch;
        }

        var structure = await structureRepository.GetOneAsync(deviceKey);
        if (structure is null)
        {
            await deviceCookieAccessor.DeleteCookieAsync();
            return AuthErrors.NoDeviceKey;
        }

        if (invitation.StructureId != structure.Id)
        {
            return AuthErrors.InvitationDoesntMatchStructure;
        }


        user.RefreshToken = Guid.NewGuid();

        var employee = new Employee()
        {
            UserId = user.Id,
            StructureId = structure.Id,
            Role = invitation.Role,
        };

        var lastSession = new LastSession()
        {
            UserId = user.Id,
            StructureId = structure.Id,
        };

        var accessToken = jwtProvider.GenerateAcessToken(
            user.Id,
            user.Role,
            structure.Id,
            employee.Role,
            employee.Id,
            out var expirationDate);

        employeeRepository.Add(employee);
        sessionRepository.Add(lastSession);
        await unitOfWork.SaveChangesAsync();
        await invitationRepository.DeleteAsync(invitation.Id);

        await refreshCookieAccessor.SetCookieAsync(user.RefreshToken);

        return new JoinResponse()
        {
            AccessToken = accessToken,
            ExpirationDate = expirationDate,
            SessionData = new()
            {
                UserId = user.Id,
                UserRole = user.Role,
                StructureId = structure.Id,
                EmployeeId = employee.Id,
                EmployeeRole = employee.Role,
            }
        };
    }
}
