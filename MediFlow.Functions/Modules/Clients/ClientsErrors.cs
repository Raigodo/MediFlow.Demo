namespace MediFlow.Functions.Modules.Clients;

public enum ClientsErrors
{
    None = 0,

    NoUser,
    NoDeviceKey,

    ClientAlreadyExists,
    ClientNotFound,
    StructureNotFound,

    UserCanNotAccessClient,
}
