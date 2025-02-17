using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Structures.Response;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public static class GetCurrentStructureEndpoint
{
    public static IEndpointRouteBuilder MapGetCurrentStructureEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/structures/current", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IStructureRepository structureRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structure = await structureRepository.GetOneAsync(currentUser.StructureId);
        if (structure is null)
        {
            return responseFactory.NotFound<Structure>();
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.StructureId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Structure>();
        }

        return responseFactory.Ok(structure.ToResponseDto());
    }
}
