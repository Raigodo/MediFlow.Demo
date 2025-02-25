using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IClientContactRepository
    {
        void Add(Contact contact);
        void Delete(Contact contact);
        Task DeleteAsync(ContactId contactId);
        Task<bool> ExistsAsync(ContactId contactId);
        Task<Contact[]> GetAllAsync(ClientId clientId);
        void Update(Contact contact);
    }
}