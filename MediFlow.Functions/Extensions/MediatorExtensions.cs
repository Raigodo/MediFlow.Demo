using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection ConfigureMediator(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                policy =>
                {
                    policy.WithOrigins(["http://localhost:3000"])
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
        });

        return services;
    }
}
