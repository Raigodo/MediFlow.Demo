using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        void Delete(Employee employee);
        Task DeleteAsync(EmployeeId employeeId);
        Task<bool> ExistsAsync(EmployeeId employeeId);
        Task<Employee[]> GetAllAsync(StructureId structureId, EmployeeRoles? employeeRole = null);
        Task<Employee?> GetOneAsync(EmployeeId employeeId);
        Task<Employee?> GetOneAsync(UserId userId, Guid DeviceKey);
        void Update(Employee employee);
    }
}