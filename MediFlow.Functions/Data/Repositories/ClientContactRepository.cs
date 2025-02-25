using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Functions.Data.Repositories;

public sealed class ClientContactRepository(AppDbContext dbContext) : IClientContactRepository
{
    public void Add(Contact contact) => dbContext.ClientContacts.Add(contact);
    public void Update(Contact contact) => dbContext.ClientContacts.Update(contact);
    public void Delete(Contact contact) => dbContext.ClientContacts.Remove(contact);

    public Task DeleteAsync(ContactId contactId) =>
        dbContext.ClientContacts
            .Where(i => i.Id == contactId)
            .ExecuteDeleteAsync();

    public Task<Contact[]> GetAllAsync(ClientId clientId) =>
        dbContext.ClientContacts
            .Where(i => i.ClientId == clientId)
            .ToArrayAsync();

    public Task<bool> ExistsAsync(ContactId contactId) =>
        dbContext.ClientContacts
            .AnyAsync(i => i.Id == contactId);
}
