using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
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