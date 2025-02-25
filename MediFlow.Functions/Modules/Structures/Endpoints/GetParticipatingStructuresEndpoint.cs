using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Functions.Modules.Structures.Endpoints;

public static class GetParticipatingStructuresEndpoint
{
    public static IEndpointRouteBuilder MapGetParticipatingStructuresEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/structures/participating", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        [FromQuery] UserId? managerId,
        ICurrentUserAccessor currentUser,
        IStructureRepository structureRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        if (managerId is { })
        {
            var canView = await accessGuard.CanViewAsync(managerId.Value);
            if (!canView)
            {
                return responseFactory.NotFound<User>();
            }
        }

        var structures = await structureRepository.GetParticipatingAsync(managerId ?? currentUser.UserId);
        return responseFactory.Ok(structures.ToResponseDto());
    }
}
