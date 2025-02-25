using Mediator;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Functions.Modules.Employees.Queries;
using Result = Result<ICollection<Employee>, EmployeesErrors>;

public record GetEmployeesQuery() : IQuery<Result>;

public class GetEmployeesQueryHandler(
    IEmployeeRepository employeeRepository,
    IStructureRepository structureRepository,
    ICurrentUserAccessor currentUser,
    IAccessGuard accessGuard) : IQueryHandler<GetEmployeesQuery, Result>
{
    public async ValueTask<Result> Handle(GetEmployeesQuery command, CancellationToken cancellationToken)
    {
        var structureExists = await structureRepository.ExistsAsync(currentUser.StructureId);
        if (!structureExists)
        {
            return EmployeesErrors.StructureNotFound;
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.StructureId);

        var employees = await employeeRepository.GetAllAsync(currentUser.StructureId);

        return employees;
    }
}
