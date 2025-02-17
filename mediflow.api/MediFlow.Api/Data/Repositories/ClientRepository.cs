using MediFlow.Api.Entities.Clients;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class ClientRepository(AppDbContext dbContext) : IClientRepository
{
    public void Add(Client client) => dbContext.Clients.Add(client);
    public void Update(Client client) => dbContext.Clients.Update(client);
    public void Delete(Client client) => dbContext.Clients.Remove(client);

    public Task DeleteAsync(ClientId clientId) =>
        dbContext.Clients
            .Where(i => i.Id == clientId)
            .ExecuteDeleteAsync();

    public Task<Client?> GetOneAsync(ClientId clientId) =>
        dbContext.Clients
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(s => s.Id == clientId);

    public Task<bool> ExistsAsync(ClientId clientId) =>
        dbContext.Clients
            .AnyAsync(s => s.Id == clientId);

    public Task<Client[]> GetAllAsync(StructureId structureId) =>
        dbContext.Clients
            .Include(c => c.Contacts)
            .Where(s => s.StructureId == structureId)
            .ToArrayAsync();
}
