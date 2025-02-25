using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IDeviceKeyRepository
    {
        void Add(DeviceKey deviceKey);
        void Delete(DeviceKey deviceKey);
        Task DeleteAsync(StructureId structureId);
        Task<DeviceKey?> GetOneAsync(StructureId structureId);
        void Update(DeviceKey deviceKey);
    }
}