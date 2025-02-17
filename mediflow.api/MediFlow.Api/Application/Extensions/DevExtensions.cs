namespace MediFlow.Api.Application.Extensions;

public static class DevExtensions
{
    public static IServiceCollection AddDevCors(this IServiceCollection services)
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

    public static IApplicationBuilder UseDevCors(this IApplicationBuilder app)
    {
        app.UseCors("AllowSpecificOrigins");

        return app;
    }
}
