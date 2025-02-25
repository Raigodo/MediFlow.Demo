using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

public sealed record SessionData(
    UserId UserId,
    StructureId StructureId,
    EmployeeRoles EmployeeRole,
    UserRoles UserRole,
    EmployeeId EmployeeId)
{
    public static readonly SessionData Empty = new(
        UserId.Create(Guid.Empty),
        StructureId.Create(Guid.Empty),
        EmployeeRoles.NotSpecified,
        UserRoles.NotSpecified,
        EmployeeId.Create(Guid.Empty));
}
