using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared;

public static class TypedIdExtensions
{
    public static bool IsEmpty(this UserId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this EmployeeRoles role) => role == EmployeeRoles.NotSpecified;
    public static bool IsEmpty(this StructureId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this EmployeeId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this ClientId id) => id.Value == string.Empty;
    public static bool IsEmpty(this NoteId id) => id.Value == Guid.Empty;
    public static bool IsEmpty(this NoteFileId id) => id.Value == Guid.Empty;
}
