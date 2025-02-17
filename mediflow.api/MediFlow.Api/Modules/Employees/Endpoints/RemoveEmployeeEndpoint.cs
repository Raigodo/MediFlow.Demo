using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Api.Modules.Employees.Endpoints;

public static class RemoveEmployeeEndpoint
{
    public static IEndpointRouteBuilder MapRemoveEmployeeEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/employees/{employeeId}", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        EmployeeId employeeId,
        IEmployeeRepository employeeRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var employeeExists = await employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            return responseFactory.NotFound<Employee>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(employeeId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Employee>();
        }

        await employeeRepository.DeleteAsync(employeeId);

        return responseFactory.NoContent();
    }
}
