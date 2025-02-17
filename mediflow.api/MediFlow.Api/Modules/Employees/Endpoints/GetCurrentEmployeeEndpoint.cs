using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Employees.Response;

namespace MediFlow.Api.Modules.Employees.Endpoints;

public static class GetCurrentEmployeeEndpoint
{
    public static IEndpointRouteBuilder MapGetCurrentEmployeeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/employees/current", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        IEmployeeRepository employeeRepository,
        ICurrentUserAccessor currentUser,
        ResponseFactory responseFactory)
    {
        var employee = await employeeRepository.GetOneAsync(currentUser.EmployeeId);
        if (employee is null)
        {
            return responseFactory.NotFound<Employee>();
        }

        return responseFactory.Ok(employee.ToResponseDto());
    }
}
