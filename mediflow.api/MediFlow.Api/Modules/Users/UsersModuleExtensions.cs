using FluentValidation;
using MediFlow.Api.Modules.Users.Endpoints;

namespace MediFlow.Api.Modules.Users;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        return services;
    }

    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Users")
            .MapCreateUserEndpoint()
            .MapGetCurrentProfileEndpoint()
            .MapGetManagersEndpoint()
            .MapGetProfileEndpoint()
            //.MapUpdateProfileEndpoint()
            .MapRemoveUserEndpoint();
        return routes;
    }
}
