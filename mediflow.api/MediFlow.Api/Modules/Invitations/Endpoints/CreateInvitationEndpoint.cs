using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Invitations.Response;

namespace MediFlow.Api.Modules.Invitations.Endpoints;

public record CreateInvitationRequest(EmployeeRoles EmployeeRole);

public static class CreateInvitationEndpoint
{
    public static IEndpointRouteBuilder MapCreateInvitationEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/employees/invitations", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        CreateInvitationRequest req,
        IStructureRepository structureRepository,
        IInvitationRepository invitationRepository,
        ICurrentUserAccessor currentUser,
        IUnitOfWork unitOfWork,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structure = await structureRepository.GetOneAsync(currentUser.StructureId);
        if (structure is null)
        {
            return responseFactory.NotFound<Structure>();
        }

        var invitation = new Invitation()
        {
            StructureId = currentUser.StructureId,
            Role = req.EmployeeRole,
            Structure = structure,
        };
        invitationRepository.Add(invitation);

        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(invitation.ToResponseDto());
    }
}
