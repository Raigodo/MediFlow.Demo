using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Entities.Structures;

public sealed class StructureManager
{
    public required StructureId StructureId { get; init; }
    public required UserId ManagerId { get; set; }


#nullable disable
    public User Manager { get; init; }
    public Structure Structure { get; init; }
#nullable restore
}
