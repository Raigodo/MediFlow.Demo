using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
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