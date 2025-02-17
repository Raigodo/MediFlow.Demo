using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Employees.Response;

namespace MediFlow.Api.Modules.Employees.Endpoints;

public static class GetEmployeeEndpoint
{
    public static IEndpointRouteBuilder MapGetEmployeeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/employees/{employeeId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        EmployeeId employeeId,
        IEmployeeRepository employeeRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var employee = await employeeRepository.GetOneAsync(employeeId);
        if (employee is null)
        {
            return responseFactory.NotFound<Employee>();
        }

        var hasAccess = await accessGuard.CanViewAsync(employeeId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Employee>();
        }

        return responseFactory.Ok(employee.ToResponseDto());
    }
}
