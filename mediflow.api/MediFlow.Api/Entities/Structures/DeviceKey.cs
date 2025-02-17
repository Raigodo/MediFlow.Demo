using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Entities.Structures;

public sealed class DeviceKey
{
    public required StructureId StructureId { get; init; }
    public required Guid KeyValue { get; set; }
    public required DateOnly KeyUpdatedOn { get; set; }


#nullable disable
    public Structure Structure { get; init; }
#nullable restore
}
