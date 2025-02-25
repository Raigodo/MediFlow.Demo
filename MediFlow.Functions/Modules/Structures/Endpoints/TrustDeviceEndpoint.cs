namespace MediFlow.Functions.Modules.Structures.Endpoints;

public static class TrustDeviceEndpoint
{
    public static IEndpointRouteBuilder MapTrustDeviceEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/structures/current/trust-device", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IStructureRepository structureRepository,
        IDeviceKeyRepository deviceKeyRepository,
        IDeviceCookieAccessor deviceCookieAccessor,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var structureExists = await structureRepository.ExistsAsync(currentUser.StructureId);
        if (!structureExists)
        {
            return responseFactory.NotFound<Structure>();
        }

        var hasAccess = await accessGuard.CanWriteAsync(currentUser.StructureId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<Structure>();
        }

        var deviceKey = await deviceKeyRepository.GetOneAsync(currentUser.StructureId);
        if (deviceKey is null)
        {
            return responseFactory.NotFound<Structure>();
        }

        await deviceCookieAccessor.SetCookieAsync(deviceKey.KeyValue);

        return responseFactory.NoContent();
    }
}
