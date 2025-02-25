using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;

namespace MediFlow.Functions.Modules.Invitations.Functions.Responses;

public record InvitationRespnseDto
{
    public required InvitationId Id { get; init; }
    public required StructureId StructureId { get; init; }
    public required string StructureName { get; init; }
    public required EmployeeRoles EmployeeRole { get; init; }
    public required DateTime IssuedAt { get; init; }
}
