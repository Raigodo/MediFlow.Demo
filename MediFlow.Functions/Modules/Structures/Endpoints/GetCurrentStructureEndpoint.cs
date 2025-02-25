namespace MediFlow.Functions.Modules.Structures.Endpoints;

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
