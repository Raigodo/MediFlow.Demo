using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
{
    public void Add(Employee employee) => dbContext.Employees.Add(employee);
    public void Update(Employee employee) => dbContext.Employees.Update(employee);
    public void Delete(Employee employee) => dbContext.Employees.Remove(employee);

    public Task DeleteAsync(EmployeeId employeeId) =>
        dbContext.Employees
            .Where(i => i.Id == employeeId)
            .ExecuteDeleteAsync();

    public Task<Employee?> GetOneAsync(EmployeeId employeeId) =>
        dbContext.Employees
            .Include(e => e.User)
            .Include(e => e.Structure)
            .FirstOrDefaultAsync(u => u.Id == employeeId);

    public Task<Employee?> GetOneAsync(UserId userId, Guid DeviceKey) =>
        dbContext.Employees
            .Include(e => e.User)
            .Include(e => e.Structure)
            .FirstOrDefaultAsync(e =>
                e.UserId == userId
                && e.Structure.DeviceKey.KeyValue == DeviceKey);

    public Task<Employee[]> GetAllAsync(StructureId structureId, EmployeeRoles? employeeRole = null)
    {
        var query = dbContext.Employees
            .Include(e => e.User)
            .Include(e => e.Structure)
            .Where(e => e.StructureId == structureId);

        if (employeeRole is not null)
            query = query.Where(e => e.Role == employeeRole);

        return query.ToArrayAsync();
    }

    public Task<bool> ExistsAsync(EmployeeId employeeId) =>
        dbContext.Employees
            .AnyAsync(u => u.Id == employeeId);


}
