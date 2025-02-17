using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

public interface ICurrentUserAccessor
{
    public SessionData CurrentUser { get; }
    public UserId UserId => CurrentUser.UserId;
    public StructureId StructureId => CurrentUser.StructureId;
    public EmployeeRoles EmployeeRole => CurrentUser.EmployeeRole;
    public UserRoles UserRole => CurrentUser.UserRole;
    public EmployeeId EmployeeId => CurrentUser.EmployeeId;
}
