using MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Functions.Modules.Auth.Services.RefreshCookie;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Extensions;

public static class SessionDataAccessorExtensions
{
    public static IServiceCollection AddSessionDataAccessors(this IServiceCollection services)
    {
        services.AddScoped<IRefreshCookieAccessor, RefreshCookieAccessor>();
        services.AddScoped<IDeviceCookieAccessor, DeviceCookieAccessor>();
        //services.AddScoped<UserDataExtractorFilter>();
        //services.AddKeyedScoped<CurrentUserAccessor>("internal");//useed by extractor filter
        //services.AddScoped<ICurrentUserAccessor>(sc => sc.GetRequiredKeyedService<CurrentUserAccessor>("internal"));
        return services;
    }
}
