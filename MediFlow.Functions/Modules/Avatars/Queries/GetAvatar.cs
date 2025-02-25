using Mediator;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Functions.Modules.Avatars.Queries;
using Result = Result<UserAvatar, AvatarErrors>;

public record GetAvatarCommand(UserId UserId) : ICommand<Result>;

public class GetAvatarCommandHandler(
    ICurrentUserAccessor currentUser,
    IUserAvatarRepository userAvatarRepository,
    IAccessGuard accessGuard) : ICommandHandler<GetAvatarCommand, Result>
{
    public async ValueTask<Result> Handle(GetAvatarCommand command, CancellationToken cancellationToken)
    {
        var userAvatar = await userAvatarRepository.GetOneAsync(currentUser.UserId);
        if (userAvatar is null)
        {
            return AvatarErrors.CurrentUserNotFound;
        }

        var hasAccess = await accessGuard.ExistsAndCanAccessAsync(command.UserId, GuardOptions.Read);
        if (!hasAccess)
        {
            return AvatarErrors.CanNotAccessAvatar;
        }

        return userAvatar;
    }
}
