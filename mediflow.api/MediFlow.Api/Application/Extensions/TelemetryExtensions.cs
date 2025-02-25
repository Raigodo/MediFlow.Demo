using Serilog;
using Serilog.Events;

namespace MediFlow.Api.Application.Extensions;

public static class TelemetryExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.File("MediFlow.Logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        return builder;
    }
}
