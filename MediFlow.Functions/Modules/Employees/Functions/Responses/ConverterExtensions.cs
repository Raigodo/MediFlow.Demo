using MediFlow.Functions.Entities.Employees;

namespace MediFlow.Functions.Modules.Employees.Functions.Responses;

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
