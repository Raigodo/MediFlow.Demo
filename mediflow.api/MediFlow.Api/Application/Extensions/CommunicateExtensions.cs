using MediFlow.Api.Modules._Shared.Services.EmailSender;
using MediFlow.Api.Modules._Shared.Services.TimeLimitedToken;

namespace MediFlow.Api.Application.Extensions;

public static class CommunicateExtensions
{
    public static IServiceCollection AddEmailSendingSupport(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
        return services;
    }

    public static IServiceCollection AddTimeLimitedTokenSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TimeLimitedTokenOptions>(configuration.GetSection("TimeLimitedToken"));
        services.AddSingleton<ITimeLimitedTokenHandler, TimeLimitedTokenHandler>();
        return services;
    }
}
