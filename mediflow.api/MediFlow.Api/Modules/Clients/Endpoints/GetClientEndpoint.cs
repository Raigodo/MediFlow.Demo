using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Clients.Response;

namespace MediFlow.Api.Modules.Clients.Endpoints;

public static class GetClientEndpoint
{
    public static IEndpointRouteBuilder MapGetClientEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/clients/{clientId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ClientId clientId,
        IClientRepository clientRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var client = await clientRepository.GetOneAsync(clientId);
        if (client is null)
        {
            return responseFactory.NotFound<Client>();
        }

        var hasAccess = await accessGuard.CanViewAsync(clientId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Client>();
        }

        return responseFactory.Ok(client.ToResponseDto());
    }
}
