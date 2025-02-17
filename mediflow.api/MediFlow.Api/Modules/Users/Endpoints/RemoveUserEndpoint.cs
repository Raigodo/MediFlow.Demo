using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Api.Modules.Users.Endpoints;

public static class RemoveUserEndpoint
{
    public static IEndpointRouteBuilder MapRemoveUserEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/users/{userId}", Handle)
            .RequireAuthorization(AuthPolicies.OnlyAdmin);
        return routes;
    }

    public static async Task<IResult> Handle(
        UserId userId,
        IUserRepository userRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var exists = await userRepository.ExistsAsync(userId);
        if (!exists)
        {
            return responseFactory.NotFound<User>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(userId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }

        await userRepository.DeleteAsync(userId);

        return responseFactory.NoContent();
    }
}
