namespace MediFlow.Functions.Modules.Invitations.Endpoints;

public static class RemoveInvitationEndpoint
{
    public static IEndpointRouteBuilder MapRemoveInvitationEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        InvitationId invitationId,
        ResponseFactory responseFactory)
    {
    }
}
