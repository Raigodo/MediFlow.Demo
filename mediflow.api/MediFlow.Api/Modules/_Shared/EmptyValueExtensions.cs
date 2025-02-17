using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared;

public static class EmptyValueExtensions
{
    public static bool IsEmpty(this UserId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this EmployeeRoles role) => role == EmployeeRoles.NotSpecified;
    public static bool IsEmpty(this StructureId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this EmployeeId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this ClientId id) => id.Value == string.Empty;
    public static bool IsEmpty(this NoteId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this NoteFileId id) => id.Value == Guid.Empty;
}
