using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Api.Modules.Avatars.Endpoints;

public static class RemoveCurrentUserAvatarEndpoint
{
    public static IEndpointRouteBuilder MapRemoveCurrentUserAvatarEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/users/current/avatar", Handle)
        .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IUserRepository userRepository,
        IUserAvatarRepository userAvatarRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var exists = await userRepository.ExistsAsync(currentUser.UserId);
        if (!exists)
        {
            return responseFactory.NotFound<User>();
        }
        var hasAccess = await accessGuard.CanWriteAsync(currentUser.UserId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }

        var userAvatarExists = await userAvatarRepository.ExistsAsync(currentUser.UserId);
        if (!userAvatarExists)
        {
            return responseFactory.NotFound<User>();
        }

        await userAvatarRepository.DeleteAsync(currentUser.UserId);

        return responseFactory.NoContent();
    }
}
