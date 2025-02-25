using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Services.AccessGuard
{
    public interface IAccessGuard
    {
        Task<bool> CanCreateClientAsync(StructureId clientId);
        Task<bool> CanViewAsync(ClientId clientId);
        Task<bool> CanWriteAsync(ClientId clientId);

        Task<bool> CanCreateNoteAsync(StructureId noteId);
        Task<bool> CanViewAsync(NoteId noteId, GuardOptions guardOption);
        Task<bool> CanWriteAsync(NoteId noteId, GuardOptions guardOption);
        Task<bool> CanViewAsync(ClientId clientId, EmployeeRoles employeeRole, GuardOptions guardOption);
        Task<bool> CanWriteAsync(ClientId clientId, EmployeeRoles employeeRole, GuardOptions guardOption);

        Task<bool> CanViewAsync(EmployeeId emplyeeId, GuardOptions guardOption);
        Task<bool> CanWriteAsync(EmployeeId emplyeeId, GuardOptions guardOption);

        Task<bool> CanCreateInvitationAsync(StructureId invitationId);
        Task<bool> CanViewAsync(InvitationId invitationId, GuardOptions guardOption);
        Task<bool> CanWriteAsync(InvitationId invitationId, GuardOptions guardOption);

        Task<bool> CanCreateStructureAsync();
        Task<bool> CanViewAsync(StructureId structureId, GuardOptions guardOption);
        Task<bool> CanWriteAsync(StructureId structureId, GuardOptions guardOption);

        Task<bool> CanCreateUserAsync(UserId userId);
        Task<bool> CanViewAsync(UserId userId, GuardOptions guardOption);
        Task<bool> CanWriteAsync(UserId userId, GuardOptions guardOption);
    }
}