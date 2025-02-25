using Microsoft.Extensions.Hosting;

namespace MediFlow.Functions.Extensions;

public static class TelemetryExtensions
{
    public static IHostApplicationBuilder AddTelemetry(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
