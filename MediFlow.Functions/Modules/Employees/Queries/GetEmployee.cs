using Mediator;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Employees.Queries;
using Result = Result<Employee, EmployeesErrors>;

public record GetEmployeeQuery(EmployeeId EmployeeId) : IQuery<Result>;

public class GetEmployeeHandler(
    IEmployeeRepository employeeRepository,
    IAccessGuard accessGuard) : IQueryHandler<GetEmployeeQuery, Result>
{
    public async ValueTask<Result> Handle(GetEmployeeQuery command, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetOneAsync(command.EmployeeId);
        if (employee is null)
        {
            return EmployeesErrors.EmployeeNotFound;
        }

        var hasAccess = await accessGuard.CanViewAsync(command.EmployeeId);
        if (!hasAccess)
        {
            return EmployeesErrors.ForbidAccessToEmployee;
        }

        return employee;
    }
}
