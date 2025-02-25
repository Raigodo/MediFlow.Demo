using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IInvitationRepository
    {
        void Add(Invitation invitation);
        void Delete(Invitation invitation);
        Task DeleteAsync(InvitationId invitationId);
        Task<bool> ExistsAsync(InvitationId invitationId);
        Task<Invitation[]> GetAllAsync(StructureId structureId);
        Task<Invitation?> GetOneAsync(InvitationId invitationId);
        void Update(Invitation invitation);
    }
}