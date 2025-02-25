using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Modules._Shared.Services;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefreshEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/refresh", Handle)
            .RequireAuthorization(AuthPolicies.SessionRefresh);
        return routes;
    }

    public static async Task<IResult> Handle(
        ResponseFactory responseFactory)
    {
        throw new NotImplementedException();
    }
}
