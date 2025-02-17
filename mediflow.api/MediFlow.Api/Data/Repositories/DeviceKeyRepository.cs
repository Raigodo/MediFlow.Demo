using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class DeviceKeyRepository(AppDbContext dbContext) : IDeviceKeyRepository
{
    public void Add(DeviceKey deviceKey) => dbContext.StructureDeviceKeys.Add(deviceKey);
    public void Update(DeviceKey deviceKey) => dbContext.StructureDeviceKeys.Update(deviceKey);
    public void Delete(DeviceKey deviceKey) => dbContext.StructureDeviceKeys.Remove(deviceKey);

    public Task DeleteAsync(StructureId structureId) =>
        dbContext.StructureDeviceKeys
            .Where(i => i.StructureId == structureId)
            .ExecuteDeleteAsync();

    public Task<DeviceKey?> GetOneAsync(StructureId structureId) =>
        dbContext.StructureDeviceKeys
            .FirstOrDefaultAsync(s => s.StructureId == structureId);
}
