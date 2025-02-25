using FluentValidation;
using MediFlow.Functions.Modules.Auth.Commands;
using MediFlow.Functions.Modules.Auth.Options;
using MediFlow.Functions.Modules.Auth.Services.JwtProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<JoinCommand>, JoinCommandValidator>();

        return services;
    }
}
