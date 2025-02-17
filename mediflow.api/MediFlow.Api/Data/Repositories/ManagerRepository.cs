using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class ManagerRepository(AppDbContext dbContext) : IManagerRepository
{
    public void Add(StructureManager manager) => dbContext.StructureManagers.Add(manager);
    public void Update(StructureManager manager) => dbContext.StructureManagers.Update(manager);
    public void Delete(StructureManager manager) => dbContext.StructureManagers.Remove(manager);

    public Task DeleteAsync(UserId userId) =>
        dbContext.StructureManagers
            .Where(i => i.ManagerId == userId)
            .ExecuteDeleteAsync();

    public Task<StructureManager?> GetOneAsync(UserId userId) =>
        dbContext.StructureManagers
            .Include(i => i.Manager)
            .Include(i => i.StructureId)
            .FirstOrDefaultAsync(s => s.ManagerId == userId);
}
