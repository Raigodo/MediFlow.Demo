using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Invitations.Response;

namespace MediFlow.Api.Modules.Invitations.Endpoints;

public static class GetInvitationsEndpoint
{
    public static IEndpointRouteBuilder MapGetInvitationsEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/employees/invitations", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        IInvitationRepository invitationRepository,
        IStructureRepository structureRepository,
        ICurrentUserAccessor currentUser,
        ResponseFactory responseFactory)
    {
        var structureExists = await structureRepository.ExistsAsync(currentUser.StructureId);
        if (!structureExists)
        {
            return responseFactory.NotFound<Structure>();
        }

        var invitation = await invitationRepository.GetAllAsync(currentUser.StructureId);

        return responseFactory.Ok(invitation.ToResponseDto());
    }
}
