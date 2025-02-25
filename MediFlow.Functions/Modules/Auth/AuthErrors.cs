namespace MediFlow.Functions.Modules.Auth;

public enum AuthErrors
{
    None = 0,

    NoDeviceKey,
    NoRefreshToken,
    CurrentUserNotFound,
    StructurByDeviceKeyNotFound,

    ForbidAccessToStructure,

    StructureNotFound,
    InvitationNotFound,
    UserByEmailNotFound,
    PasswordDoesntMatch,
    UserNotEnrolledInStructure,
    InvitationDoesntMatchStructure,
    InvitationInvalid,
    EmailAlreadyTaken,
}