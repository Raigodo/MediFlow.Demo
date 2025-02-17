using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules.Users.Response;

namespace MediFlow.Api.Modules.Users.Endpoints;

public static class GetManagersEndpoint
{
    public static IEndpointRouteBuilder MapGetManagersEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/managers", Handle)
            .RequireAuthorization(AuthPolicies.OnlyAdmin);
        return routes;
    }

    public static async Task<IResult> Handle(
        IUserRepository userRepository,
        ResponseFactory responseFactory)
    {
        var managers = await userRepository.GetManyAsync(UserRoles.Manager);
        return responseFactory.Ok(managers.ToResponseDto());
    }
}
