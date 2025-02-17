using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Api.Modules.Invitations.Endpoints;

public static class RemoveInvitationEndpoint
{
    public static IEndpointRouteBuilder MapRemoveInvitationEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/employees/invitations/{invitationId}", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        InvitationId invitationId,
        IInvitationRepository invitationRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var invitationExists = await invitationRepository.ExistsAsync(invitationId);
        if (!invitationExists)
        {
            return responseFactory.NotFound<Invitation>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(invitationId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Invitation>();
        }

        await invitationRepository.DeleteAsync(invitationId);

        return responseFactory.NoContent();
    }
}
