using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Entities.Structures.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IClientRepository
    {
        void Add(Client client);
        void Delete(Client client);
        Task DeleteAsync(ClientId clientId);
        Task<bool> ExistsAsync(ClientId clientId);
        Task<Client[]> GetAllAsync(StructureId structureId);
        Task<Client?> GetOneAsync(ClientId clientId);
        void Update(Client client);
    }
}