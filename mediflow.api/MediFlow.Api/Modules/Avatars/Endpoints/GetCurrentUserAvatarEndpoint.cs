using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Api.Modules.Avatars.Endpoints;

public static class GetCurrentUserAvatarEndpoint
{
    public static IEndpointRouteBuilder MapGetCurrentUserAvatarEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/current/avatar", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IUserAvatarRepository userAvatarRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var userAvatar = await userAvatarRepository.GetOneAsync(currentUser.UserId);
        if (userAvatar is null)
        {
            return responseFactory.NotFound<User>();
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.UserId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }
        new FileContentResult(userAvatar.Data, userAvatar.ContentType)
        {
            FileDownloadName = userAvatar.FileName
        };

        return TypedResults.File(userAvatar.Data, userAvatar.ContentType, userAvatar.FileName);
    }
}
