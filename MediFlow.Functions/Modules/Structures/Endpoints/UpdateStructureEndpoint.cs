namespace MediFlow.Functions.Modules.Structures.Endpoints;

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
