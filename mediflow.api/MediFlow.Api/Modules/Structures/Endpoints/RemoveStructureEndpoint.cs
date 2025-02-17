using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public static class RemoveStructureEndpoint
{
    public static IEndpointRouteBuilder MapRemoveStructureEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/structures/{structureId}", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        StructureId structureId,
        IStructureRepository structureRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structureExists = await structureRepository.ExistsAsync(structureId);
        if (!structureExists)
        {
            return responseFactory.NotFound<Structure>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(structureId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Structure>();
        }
        await structureRepository.DeleteAsync(structureId);

        return responseFactory.NoContent();
    }
}
