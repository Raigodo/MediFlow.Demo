using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

public interface ICurrentUserAccessor
{
    public SessionData CurrentUser { get; }
    public UserId UserId => CurrentUser.UserId;
    public StructureId StructureId => CurrentUser.StructureId;
    public EmployeeRoles EmployeeRole => CurrentUser.EmployeeRole;
    public UserRoles UserRole => CurrentUser.UserRole;
    public EmployeeId EmployeeId => CurrentUser.EmployeeId;
}
