using Mediator;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace MediFlow.Functions.Modules.Avatars.Commands;
using Result = Result<UserAvatar, AvatarErrors>;

public record SetAvatarCommand(UserId UserId, IFormFile file) : ICommand<Result>;

public class SetAvatarCommandHandler(
        ICurrentUserAccessor currentUser,
        IUserAvatarRepository userAvatarRepository,
        IUnitOfWork unitOfWork,
        IAccessGuard accessGuard) : ICommandHandler<SetAvatarCommand, Result>
{
    public async ValueTask<Result> Handle(SetAvatarCommand command, CancellationToken cancellationToken)
    {
        var userExists = await accessGuard.ExistsAndCanAccessAsync(command.UserId, GuardOptions.Update);
        if (userExists)
        {
            return AvatarErrors.CanNotAccessAvatar;
        }

        if (command.file == null || command.file.Length == 0)
            return AvatarErrors.ImageMissing;

        using var memoryStream = new MemoryStream();
        await command.file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        try
        {
            int maxWidth = 512;
            int maxHeight = 512;
            using var image = await Image.LoadAsync(memoryStream);

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                return AvatarErrors.ImageTooBig;
            }
        }
        catch
        {
            return AvatarErrors.ImageInvalid;
        }

        var fileBytes = memoryStream.ToArray();

        var userAvatar = new UserAvatar()
        {
            UserId = currentUser.UserId,
            FileName = command.file.FileName,
            ContentType = command.file.ContentType,
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

        return userAvatar;
    }
}
