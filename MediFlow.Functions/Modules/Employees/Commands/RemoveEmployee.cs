using Mediator;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Employees.Commands;
using Result = Result<Unit, EmployeesErrors>;

public record RemoveEmployeeCommand(EmployeeId EmployeeId) : ICommand<Result>;

public class RemoveEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IAccessGuard accessGuard) : ICommandHandler<RemoveEmployeeCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeExists = await employeeRepository.ExistsAsync(command.EmployeeId);
        if (!employeeExists)
        {
            return EmployeesErrors.EmployeeNotFound;
        }

        var hasAccess = await accessGuard.CanWriteAsync(command.EmployeeId);
        if (!hasAccess)
        {
            return EmployeesErrors.ForbidAccessToEmployee;
        }

        await employeeRepository.DeleteAsync(command.EmployeeId);

        return Unit.Value;
    }
}
