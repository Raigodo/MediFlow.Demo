using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;

namespace MediFlow.Functions.Entities.Employees;

public class Invitation
{
    public InvitationId Id { get; init; } = InvitationId.Generate();
    public required StructureId StructureId { get; init; }
    public EmployeeRoles Role { get; init; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;


#nullable disable
    public Structure Structure { get; init; }
#nullable restore
}
