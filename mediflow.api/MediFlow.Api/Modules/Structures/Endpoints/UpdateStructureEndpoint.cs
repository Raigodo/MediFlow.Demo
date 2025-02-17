using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules.Structures.Response;

namespace MediFlow.Api.Modules.Structures.Endpoints;

public record UpdateStructureRequest(string StructureName);

public static class UpdateStructureEndpoint
{
    public static IEndpointRouteBuilder MapUpdateStructureEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPatch("/api/structures/current", Handle)
        .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        StructureId structureId,
        UpdateStructureRequest req,
        IStructureRepository structureRepository,
        IUnitOfWork unitOfWork,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structure = await structureRepository.GetOneAsync(structureId);
        if (structure is null)
        {
            return responseFactory.NotFound<Structure>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(structureId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Structure>();
        }

        structure.Name = req.StructureName;
        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(structure.ToResponseDto());
    }
}
