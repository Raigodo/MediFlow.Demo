namespace MediFlow.Functions.Modules.Invitations.Endpoints;

public record CreateInvitationRequest();

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
        ResponseFactory responseFactory)
    {
    }
}
