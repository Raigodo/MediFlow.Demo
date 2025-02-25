namespace MediFlow.Functions.Modules.Avatars;

public enum AvatarErrors
{
    None = 0,

    NoUser,
    NoDeviceKey,
    CurrentUserNotFound,
    UserHasNoAvatar,

    ImageTooBig,
    ImageInvalid,
    ImageMissing,

    CanNotAccessAvatar,
}
