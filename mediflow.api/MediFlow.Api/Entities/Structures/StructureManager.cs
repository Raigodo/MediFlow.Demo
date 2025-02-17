using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Structures;

public sealed class StructureManager
{
    public required StructureId StructureId { get; init; }
    public required UserId ManagerId { get; set; }


#nullable disable
    public User Manager { get; init; }
    public Structure Structure { get; init; }
#nullable restore
}
