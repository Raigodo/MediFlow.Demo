using MediFlow.Api.Data;
using MediFlow.Api.Data.Options;
using MediFlow.Api.Data.Repositories;
using MediFlow.Api.Data.Services.DataEncryptor;
using MediFlow.Api.Data.Services.PasswordHasher;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Application.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddDatabaseSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IClientContactRepository, ClientContactRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDeviceKeyRepository, DeviceKeyRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IManagerRepository, ManagerRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IStructureRepository, StructureRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAvatarRepository, UserAvatarRepository>();

        return services;
    }

    public static IHost EnsureDatabaseUpdated(this IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        return app;
    }

    public static IServiceCollection AddEnryptionSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataSecurityOptions>(configuration.GetSection("DataSecurity"));
        services.AddSingleton<IDataEncryptor, DataEncryptor>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
