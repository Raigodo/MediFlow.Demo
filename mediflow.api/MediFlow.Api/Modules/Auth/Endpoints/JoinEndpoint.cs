using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Modules._Shared.Services;

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
        ResponseFactory responseFactory)
    {
        throw new NotImplementedException();
    }
}
