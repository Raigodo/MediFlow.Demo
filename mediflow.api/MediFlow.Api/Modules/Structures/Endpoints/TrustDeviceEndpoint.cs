using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;

namespace MediFlow.Api.Modules.Structures.Endpoints;

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
