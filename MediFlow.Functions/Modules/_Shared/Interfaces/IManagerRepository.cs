using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IManagerRepository
    {
        void Add(StructureManager manager);
        void Delete(StructureManager manager);
        Task DeleteAsync(UserId userId);
        Task<StructureManager?> GetOneAsync(UserId userId);
        void Update(StructureManager manager);
    }
}