using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Services.AccessGuard
{
    public interface IAccessGuard
    {
        Task<bool> CanViewAsync(ClientId clientId);
        Task<bool> CanViewAsync(ClientId clientId, EmployeeRoles employeeRole);
        Task<bool> CanViewAsync(EmployeeId employeeId);
        Task<bool> CanViewAsync(InvitationId invitationId);
        Task<bool> CanViewAsync(NoteId noteId);
        Task<bool> CanViewAsync(StructureId structureId);
        Task<bool> CanViewAsync(UserId userId);
        Task<bool> CanWriteAsync(ClientId clientId);
        Task<bool> CanWriteAsync(ClientId clientId, EmployeeRoles employeeRole);
        Task<bool> CanWriteAsync(EmployeeId employeeId);
        Task<bool> CanWriteAsync(InvitationId invitationId);
        Task<bool> CanWriteAsync(NoteId noteId);
        Task<bool> CanWriteAsync(StructureId structureId);
        Task<bool> CanWriteAsync(UserId userId);
    }
}