namespace MediFlow.Functions.Modules.Structures.Endpoints;

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
