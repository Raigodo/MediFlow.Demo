using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class StructureRepository(AppDbContext dbContext) : IStructureRepository
{
    public void Add(Structure structure) => dbContext.Structures.Add(structure);
    public void Update(Structure structure) => dbContext.Structures.Update(structure);
    public void Delete(Structure structure) => dbContext.Structures.Remove(structure);

    public Task DeleteAsync(StructureId structureId) =>
        dbContext.Structures
            .Where(i => i.Id == structureId)
            .ExecuteDeleteAsync();

    public Task<Structure?> GetOneAsync(StructureId structureId) =>
        dbContext.Structures
            .Include(s => s.Manager)
            .Include(s => s.DeviceKey)
            .FirstOrDefaultAsync(s => s.Id == structureId);

    public Task<Structure?> GetOneAsync(Guid deviceKey) =>
        dbContext.Structures
            .Include(s => s.Manager)
            .Include(s => s.DeviceKey)
            .FirstOrDefaultAsync(s => s.DeviceKey.KeyValue == deviceKey);

    public Task<bool> ExistsAsync(StructureId structureId) =>
        dbContext.Structures
            .AnyAsync(s => s.Id == structureId);
    public Task<bool> ExistsAsync(Guid deviceKey) =>
        dbContext.Structures
            .AnyAsync(s => s.DeviceKey.KeyValue == deviceKey);

    public Task<Structure[]> GetParticipatingAsync(UserId userId) =>
        dbContext.Structures
            .Include(s => s.Manager)
            .Include(s => s.DeviceKey)
            .Where(s =>
                s.ManagerId == userId
                || s.Employees.Any(e => e.UserId == userId))
            .ToArrayAsync();

    public Task<Structure[]> GetOwnedAsync(UserId userId) =>
        dbContext.Structures
            .Include(s => s.Manager)
            .Include(s => s.DeviceKey)
            .Where(sm => sm.ManagerId == userId)
            .ToArrayAsync();
}