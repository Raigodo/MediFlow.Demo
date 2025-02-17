using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

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
