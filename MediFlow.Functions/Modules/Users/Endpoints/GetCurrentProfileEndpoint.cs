namespace MediFlow.Functions.Modules.Users.Endpoints;

public static class GetCurrentProfileEndpoint
{
    public static IEndpointRouteBuilder MapGetCurrentProfileEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/current", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IUserRepository userRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return responseFactory.NotFound<User>();
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.UserId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }

        return responseFactory.Ok(user.ToResponseDto());
    }
}
