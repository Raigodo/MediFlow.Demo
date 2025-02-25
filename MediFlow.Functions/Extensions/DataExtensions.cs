using MediFlow.Functions.Data;
using MediFlow.Functions.Data.Options;
using MediFlow.Functions.Data.Repositories;
using MediFlow.Functions.Data.Services.DataEncryptor;
using MediFlow.Functions.Data.Services.PasswordHasher;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediFlow.Functions.Extensions;

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
                context.Database.EnsureCreated();
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
