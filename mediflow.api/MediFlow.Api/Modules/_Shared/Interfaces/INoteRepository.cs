using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Journal.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
{
    public interface INoteRepository
    {
        void Add(Note manager);
        void Delete(Note manager);
        Task DeleteAsync(ClientId clientId, EmployeeRoles employeeRole);
        Task<bool> ExistsAsync(ClientId clientId, EmployeeRoles employeeRole);
        Task<bool> ExistsAsync(NoteId noteId);
        Task<Note[]> GetManyAsync(ClientId clientId, EmployeeRoles role, DateOnly? from, DateOnly? to, bool? isFlagged, int skipCount, int takeCount);
        Task<Note?> GetOneAsync(ClientId clientId, EmployeeRoles employeeRole);
        Task<Note?> GetOneAsync(NoteId noteId);
        void Update(Note manager);
    }
}