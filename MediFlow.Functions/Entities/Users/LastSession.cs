using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Entities.Users;

public sealed class LastSession
{
    public required UserId UserId { get; init; }
    public required StructureId StructureId { get; set; }


#nullable disable
    public User User { get; init; }
    public Structure Structure { get; init; }
#nullable restore
}
