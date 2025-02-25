using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Api.Modules.Avatars.Endpoints;

public static class RemoveCurrentUserAvatarEndpoint
{
    public static IEndpointRouteBuilder MapRemoveCurrentUserAvatarEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/users/current/avatar", Handle)
        .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }
    public static async Task<IResult> Handle(
        ICurrentUserAccessor currentUser,
        ResponseFactory responseFactory)
    {
        throw new NotImplementedException();
    }
}
