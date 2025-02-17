using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.PasswordHasher;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Api.Modules.Users.Response;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record RegisterRequest(
        string UserName,
        string UserSurname,
        string Email,
        string Password,
        InvitationId InvitationId);

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/register", Handle)
            .RequireAuthorization(AuthPolicies.Device);
        return routes;
    }

    public static async Task<IResult> Handle(
        RegisterRequest req,
        IUserRepository userRepository,
        IEmployeeRepository emploeeRepository,
        IStructureRepository structureRepository,
        IInvitationRepository invitationRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IDeviceCookieAccessor deviceCookieAccessor,
        ResponseFactory responseFactory)
    {
        var invitation = await invitationRepository.GetOneAsync(req.InvitationId);
        if (invitation is null)
        {
            return responseFactory.BadRequest(new()
            {
                { nameof(InvitationId), ["Provided invitation is incorrect or expired."] }
            });
        }

        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);
        if (!hasDeviceKey)
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

        var user = new User
        {
            Name = req.UserName,
            Surname = req.UserSurname,
            PasswordHash = passwordHasher.Generate(req.Password),
            Email = req.Email,
            Role = UserRoles.Employee,
        };

        var employee = new Employee()
        {
            UserId = user.Id,
            StructureId = structure.Id,
            Role = invitation.Role,
        };

        userRepository.Add(user);
        emploeeRepository.Add(employee);
        await unitOfWork.SaveChangesAsync();
        await invitationRepository.DeleteAsync(invitation.Id);

        return responseFactory.Ok(user.ToResponseDto());
    }
}
