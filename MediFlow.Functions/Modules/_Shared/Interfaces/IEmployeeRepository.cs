using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
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