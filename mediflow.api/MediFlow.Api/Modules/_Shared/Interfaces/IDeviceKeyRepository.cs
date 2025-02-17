using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
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