using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;

namespace MediFlow.Api.Modules.Invitations.Response;

public record InvitationRespnseDto
{
    public required InvitationId Id { get; init; }
    public required StructureId StructureId { get; init; }
    public required string StructureName { get; init; }
    public required EmployeeRoles EmployeeRole { get; init; }
    public required DateTime IssuedAt { get; init; }
}
