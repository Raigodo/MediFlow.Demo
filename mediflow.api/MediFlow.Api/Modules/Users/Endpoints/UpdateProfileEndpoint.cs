using MediFlow.Api.Application.Auth.Values;

namespace MediFlow.Api.Modules.Users.Endpoints;

public static class UpdateProfileEndpoint
{
    public static IEndpointRouteBuilder MapUpdateProfileEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPatch("/api/users/{userId}/profile", Handle)
            .RequireAuthorization(AuthPolicies.EmployeePlus);
        return routes;
    }

    public static Task<IResult> Handle()
    {
        throw new NotImplementedException();
    }
}
