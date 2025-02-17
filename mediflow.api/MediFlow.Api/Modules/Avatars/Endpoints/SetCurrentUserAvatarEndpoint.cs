using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using SixLabors.ImageSharp;

namespace MediFlow.Api.Modules.Avatars.Endpoints;

public static class SetCurrentUserAvatarEndpoint
{
    public static IEndpointRouteBuilder MapSetCurrentUserAvatarEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPut("/api/users/current/avatar", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        IFormFile file,
        ICurrentUserAccessor currentUser,
        IUserRepository userRepository,
        IUserAvatarRepository userAvatarRepository,
        IUnitOfWork unitOfWork,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        if (file == null || file.Length == 0)
            return responseFactory.BadRequest("No file uploaded.");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        try
        {
            int maxWidth = 512;
            int maxHeight = 512;
            using var image = await Image.LoadAsync(memoryStream);

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                return responseFactory.BadRequest("Image is too big");
            }
        }
        catch
        {
            return responseFactory.BadRequest();
        }

        var fileBytes = memoryStream.ToArray();

        var userAvatar = new UserAvatar()
        {
            UserId = currentUser.UserId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = fileBytes,
        };

        var userAvatarExists = await userAvatarRepository.ExistsAsync(currentUser.UserId);

        if (userAvatarExists)
        {
            userAvatarRepository.Update(userAvatar);
        }
        else
        {
            userAvatarRepository.Add(userAvatar);
        }

        await unitOfWork.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
