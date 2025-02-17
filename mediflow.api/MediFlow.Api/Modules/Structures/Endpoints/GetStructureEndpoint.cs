using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Structures.Response;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public static class GetStructureEndpoint
{
    public static IEndpointRouteBuilder MapGetStructureEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/structures/{structureId}", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        StructureId structureId,
        IStructureRepository structureRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structure = await structureRepository.GetOneAsync(structureId);
        if (structure is null)
        {
            return responseFactory.NotFound<Structure>();
        }

        var hasAccess = await accessGuard.CanViewAsync(structureId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Structure>();
        }

        return responseFactory.Ok(structure.ToResponseDto());
    }
}
