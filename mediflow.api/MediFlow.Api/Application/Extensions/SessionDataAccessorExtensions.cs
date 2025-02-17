using MediFlow.Api.Application.Pipelines;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Api.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Api.Application.Extensions;

public static class SessionDataAccessorExtensions
{
    public static IServiceCollection AddSessionDataAccessors(this IServiceCollection services)
    {
        services.AddScoped<UserDataExtractorFilter>();
        services.AddScoped<IRefreshCookieAccessor, RefreshCookieAccessor>();
        services.AddScoped<IDeviceCookieAccessor, DeviceCookieAccessor>();
        services.AddKeyedScoped<CurrentUserAccessor>("internal");//useed by extractor filter
        services.AddScoped<ICurrentUserAccessor>(sc => sc.GetRequiredKeyedService<CurrentUserAccessor>("internal"));
        return services;
    }
}
