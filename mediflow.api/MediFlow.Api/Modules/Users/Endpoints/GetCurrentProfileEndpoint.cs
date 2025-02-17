using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Users.Response;

namespace MediFlow.Api.Modules.Users.Endpoints;

public static class GetCurrentProfileEndpoint
{
    public static IEndpointRouteBuilder MapGetCurrentProfileEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/current", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        IUserRepository userRepository,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return responseFactory.NotFound<User>();
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.UserId);
        if (!hasAccess)
        {
            return responseFactory.NotFound<User>();
        }

        return responseFactory.Ok(user.ToResponseDto());
    }
}
