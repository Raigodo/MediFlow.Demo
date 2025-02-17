using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Employees.Response;

namespace MediFlow.Api.Modules.Employees.Endpoints;


public static class GetEmployeesEndpoint
{
    public static IEndpointRouteBuilder MapGetEmployeesEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/employees", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        IEmployeeRepository employeeRepository,
        IStructureRepository structureRepository,
        ICurrentUserAccessor currentUser,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structureExists = await structureRepository.ExistsAsync(currentUser.StructureId);
        if (!structureExists)
        {
            return responseFactory.NotFound<Structure>();
        }

        var employees = await employeeRepository.GetAllAsync(currentUser.StructureId);

        return responseFactory.Ok(employees.ToResponseDto());
    }
}
