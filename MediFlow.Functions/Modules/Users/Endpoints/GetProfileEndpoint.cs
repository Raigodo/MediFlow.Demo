namespace MediFlow.Functions.Modules.Users.Endpoints;

public static class GetProfileEndpoint
{
    public static IEndpointRouteBuilder MapGetProfileEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/{userId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        UserId userId,
        IUserRepository userRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var user = await userRepository.GetOneAsync(userId);
        if (user is null)
        {
            return responseFactory.NotFound<User>();
        }

        var hasAccess = await accessGuard.CanViewAsync(userId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }

        return responseFactory.Ok(user.ToResponseDto());
    }
}
