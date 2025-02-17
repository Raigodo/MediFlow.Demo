using MediFlow.Api.Entities.Employees;

namespace MediFlow.Api.Modules.Employees.Response;

public static class ConverterExtensions
{
    public static IEnumerable<EmployeeResponseDto> ToResponseDto(this IEnumerable<Employee> emmployees)
    {
        return emmployees.Select(e => e.ToResponseDto());
    }

    public static EmployeeResponseDto ToResponseDto(this Employee employee)
    {
        return new EmployeeResponseDto()
        {
            Id = employee.Id,
            UserId = employee.UserId,
            Name = employee.User?.Name,
            Surname = employee.User?.Surname,
            StructureId = employee.StructureId,
            Role = employee.Role,
            AssignedOn = employee.AssignedOn,
            UnassignedOn = employee.UnassignedOn,
        };
    }
}
