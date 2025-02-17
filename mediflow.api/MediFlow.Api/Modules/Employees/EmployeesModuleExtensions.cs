using MediFlow.Api.Modules.Employees.Endpoints;

namespace MediFlow.Api.Modules.Employees;

public static class EmployeesModuleExtensions
{
    public static IEndpointRouteBuilder MapEmployeesEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Employees")
            .MapGetCurrentEmployeeEndpoint()
            .MapGetEmployeeEndpoint()
            .MapGetEmployeesEndpoint()
            .MapRemoveEmployeeEndpoint();
        return routes;
    }
}
