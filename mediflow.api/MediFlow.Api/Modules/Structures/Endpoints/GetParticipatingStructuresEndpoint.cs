using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Structures.Response;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Api.Modules.Structures.Endpoints;

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
