using FluentValidation;
using MediFlow.Api.Modules.Clients.Endpoints;

namespace MediFlow.Api.Modules.Clients;

public static class ClientsModuleExtensions
{
    public static IServiceCollection AddClientsModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddClientRequest>, AddClientRequestValidator>();
        services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientRequestValidator>();
        return services;
    }

    public static IEndpointRouteBuilder MapClientsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Clients")
            .MapGetClientEndpoint()
            .MapGetClientsEndpoint()
            .MapAddClientEndpoint()
            .MapUpdateClientEndpoint()
            .MapRemoveClientEndpoint();
        return routes;
    }
}
