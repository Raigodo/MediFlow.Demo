using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record AlterResponse
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