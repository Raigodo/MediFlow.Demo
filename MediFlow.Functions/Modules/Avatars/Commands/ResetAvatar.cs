using Mediator;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Functions.Modules.Avatars.Commands;
using Result = Result<Unit, AvatarErrors>;

public record ResetAvatarCommand(UserId UserId) : ICommand<Result>;

public class ResetAvatarCommandHandler(
    ICurrentUserAccessor currentUser,
    IUserAvatarRepository userAvatarRepository,
    IAccessGuard accessGuard) : ICommandHandler<ResetAvatarCommand, Result>
{
    public async ValueTask<Result> Handle(ResetAvatarCommand command, CancellationToken cancellationToken)
    {
        var hasAccess = await accessGuard.ExistsAndCanAccessAsync(currentUser.UserId, GuardOptions.Delete);
        if (!hasAccess)
        {
            return AvatarErrors.CanNotAccessAvatar;
        }

        var userAvatarExists = await userAvatarRepository.ExistsAsync(currentUser.UserId);
        if (!userAvatarExists)
        {
            return AvatarErrors.UserHasNoAvatar;
        }

        await userAvatarRepository.DeleteAsync(currentUser.UserId);

        return Unit.Value;
    }
}
