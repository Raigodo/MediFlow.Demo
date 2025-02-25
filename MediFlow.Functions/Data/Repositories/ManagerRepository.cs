using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Functions.Data.Repositories;

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
