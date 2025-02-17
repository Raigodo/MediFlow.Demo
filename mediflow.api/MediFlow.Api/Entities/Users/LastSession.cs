using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Users;

public sealed class LastSession
{
    public required UserId UserId { get; init; }
    public required StructureId StructureId { get; set; }


#nullable disable
    public User User { get; init; }
    public Structure Structure { get; init; }
#nullable restore
}
