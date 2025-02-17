using FluentValidation;
using MediFlow.Api.Modules.Auth.Endpoints;
using MediFlow.Api.Modules.Auth.Options;
using MediFlow.Api.Modules.Auth.Services.JwtProvider;

namespace MediFlow.Api.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddScoped<IValidator<JoinRequest>, JoinRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

        return services;
    }

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Auth")
            .MapLoginEndpoint()
            .MapJoinEndpoint()
            .MapRefreshEndpoint()
            .MapAlterEndpoint()
            .MapRegisterEndpoint();
        return routes;
    }
}
