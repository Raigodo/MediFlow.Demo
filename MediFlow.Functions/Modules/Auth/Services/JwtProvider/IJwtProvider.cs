using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules.Auth.Services.JwtProvider
{
    public interface IJwtProvider
    {
        string GenerateAcessToken(UserId userId, UserRoles userRole, out DateTime expirationDate);
        string GenerateAcessToken(UserId userId, UserRoles userRole, StructureId structureId, EmployeeRoles employeeRole, EmployeeId employeeId, out DateTime expirationDate);
        string GenerateAcessToken(UserId userId, UserRoles userRole, StructureId structureId, out DateTime expirationDate);
    }
}