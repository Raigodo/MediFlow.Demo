using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules.Auth.Services.JwtProvider
{
    public interface IJwtProvider
    {
        string GenerateAcessToken(UserId userId, UserRoles userRole, out DateTime expirationDate);
        string GenerateAcessToken(UserId userId, UserRoles userRole, StructureId structureId, EmployeeRoles employeeRole, EmployeeId employeeId, out DateTime expirationDate);
        string GenerateAcessToken(UserId userId, UserRoles userRole, StructureId structureId, out DateTime expirationDate);
    }
}