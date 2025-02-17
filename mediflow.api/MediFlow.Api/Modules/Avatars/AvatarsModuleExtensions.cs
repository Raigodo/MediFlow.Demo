using MediFlow.Api.Modules.Avatars.Endpoints;

namespace MediFlow.Api.Modules.Avatars;

public static class AvatarsModuleExtensions
{
    public static IEndpointRouteBuilder MapAvatarsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Users")
            .MapGetCurrentUserAvatarEndpoint()
            .MapGetUserAvatarEndpoint()
            .MapSetCurrentUserAvatarEndpoint()
            .MapRemoveCurrentUserAvatarEndpoint();
        return routes;
    }
}
