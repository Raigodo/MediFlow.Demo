namespace MediFlow.Functions.Modules.Structures.Endpoints;

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
