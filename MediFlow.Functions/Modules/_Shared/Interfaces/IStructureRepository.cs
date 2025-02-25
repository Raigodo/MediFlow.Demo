using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IStructureRepository
    {
        void Add(Structure structure);
        void Delete(Structure structure);
        Task DeleteAsync(StructureId structureId);
        Task<bool> ExistsAsync(StructureId structureId);
        Task<bool> ExistsAsync(Guid deviceKey);
        Task<Structure?> GetOneAsync(Guid deviceKey);
        Task<Structure?> GetOneAsync(StructureId structureId);
        Task<Structure[]> GetOwnedAsync(UserId userId);
        Task<Structure[]> GetParticipatingAsync(UserId userId);
        void Update(Structure structure);
    }
}