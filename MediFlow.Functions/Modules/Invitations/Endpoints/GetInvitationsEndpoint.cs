namespace MediFlow.Functions.Modules.Invitations.Endpoints;

public static class GetInvitationsEndpoint
{
    public static IEndpointRouteBuilder MapGetInvitationsEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/employees/invitations", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ResponseFactory responseFactory)
    {
    }
}
