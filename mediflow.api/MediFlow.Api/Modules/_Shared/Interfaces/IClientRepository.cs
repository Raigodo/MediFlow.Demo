using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
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