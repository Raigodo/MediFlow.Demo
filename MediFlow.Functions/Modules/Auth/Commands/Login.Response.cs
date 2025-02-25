using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules.Auth.Commands;

public record LoginResponse
{
    public required string AccessToken { get; init; }
    public required DateTime ExpirationDate { get; init; }
    public required SessionDataDto SessionData { get; init; }


    public readonly record struct SessionDataDto
    {
        public required readonly UserId UserId { get; init; }
        public required readonly UserRoles UserRole { get; init; }
        public required readonly StructureId StructureId { get; init; }
        public required readonly EmployeeRoles EmployeeRole { get; init; }
        public required readonly EmployeeId EmployeeId { get; init; }
    }
}
