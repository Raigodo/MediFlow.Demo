using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules.Employees.Functions.Responses;

public record EmployeeResponseDto
{
    public required EmployeeId Id { get; init; }
    public required UserId? UserId { get; init; }
    public required StructureId StructureId { get; init; }
    public required string? Name { get; init; }
    public required string? Surname { get; init; }
    public required EmployeeRoles Role { get; init; }
    public required DateOnly AssignedOn { get; init; }
    public required DateOnly? UnassignedOn { get; init; }
}
