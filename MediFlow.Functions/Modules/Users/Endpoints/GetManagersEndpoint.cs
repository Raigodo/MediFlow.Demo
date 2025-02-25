namespace MediFlow.Functions.Modules.Users.Endpoints;

public static class GetManagersEndpoint
{
    public static IEndpointRouteBuilder MapGetManagersEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/managers", Handle)
            .RequireAuthorization(AuthPolicies.OnlyAdmin);
        return routes;
    }

    public static async Task<IResult> Handle(
        IUserRepository userRepository,
        ResponseFactory responseFactory)
    {
        var managers = await userRepository.GetManyAsync(UserRoles.Manager);
        return responseFactory.Ok(managers.ToResponseDto());
    }
}
