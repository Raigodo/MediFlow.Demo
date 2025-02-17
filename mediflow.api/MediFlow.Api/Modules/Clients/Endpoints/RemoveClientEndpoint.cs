using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Api.Modules.Clients.Endpoints;


public static class RemoveClientEndpoint
{
    public static IEndpointRouteBuilder MapRemoveClientEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/clients/{ClientId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        IClientRepository clientRepository,
        IStructureRepository structureRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var exists = await clientRepository.ExistsAsync(clientId);
        if (!exists)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(clientId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        await clientRepository.DeleteAsync(clientId);

        return responseFactory.NoContent();
    }
}
