using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Services;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record AlterRequest(StructureId StructureId);

public static class AlterEndpoint
{
    public static IEndpointRouteBuilder MapAlterEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/alter", Handle)
            .RequireAuthorization(AuthPolicies.SessionMutation);
        return routes;
    }

    public static async Task<IResult> Handle(
        AlterRequest req,
        ResponseFactory responseFactory)
    {
        throw new NotImplementedException();
    }
}
