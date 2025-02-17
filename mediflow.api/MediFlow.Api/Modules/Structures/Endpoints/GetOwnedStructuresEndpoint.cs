using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Structures.Response;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public static class GetOwnedStructuresEndpoint
{
    public static IEndpointRouteBuilder MapGetOwnedStructuresEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/structures/owned", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IStructureRepository structureRepository,
        ResponseFactory responseFactory)
    {
        var structures = await structureRepository.GetOwnedAsync(currentUser.UserId);
        return responseFactory.Ok(structures.ToResponseDto());
    }
}
