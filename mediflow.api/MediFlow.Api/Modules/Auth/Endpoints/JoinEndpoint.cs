using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.PasswordHasher;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Api.Modules.Auth.Services.JwtProvider;
using MediFlow.Api.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record JoinRequest(string Email, string Password, InvitationId InvitationId);

public static class JoinEndpoint
{
    public static IEndpointRouteBuilder MapJoinEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/join", Handle)
            .RequireAuthorization(AuthPolicies.Device);
        return routes;
    }

    public static async Task<IResult> Handle(
        JoinRequest req,
        IUserRepository userRepository,
        IEmployeeRepository employeeRepository,
        IStructureRepository structureRepository,
        IInvitationRepository invitationRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ISessionRepository sessionRepository,
        IJwtProvider jwtProvider,
        IRefreshCookieAccessor refreshCookieAccessor,
        IDeviceCookieAccessor deviceCookieAccessor,
        ResponseFactory responseFactory)
    {
        var invitation = await invitationRepository.GetOneAsync(req.InvitationId);
        if (invitation is null)
        {
            return responseFactory.Unauthorized();
        }

        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);

        var user = await userRepository.GetOneAsync(req.Email);
        if (user is null)
        {
            return responseFactory.Unauthorized();
        }

        if (!passwordHasher.Veriffy(req.Password, user.PasswordHash))
        {
            return responseFactory.Unauthorized();
        }

        var structure = await structureRepository.GetOneAsync(deviceKey);
        if (structure is null)
        {
            await deviceCookieAccessor.DeleteCookieAsync();
            return responseFactory.Unauthorized();
        }

        if (invitation.StructureId != structure.Id)
        {
            return responseFactory.Unauthorized();
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

        return responseFactory.Ok(new JoinResponse()
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
        });
    }
}
