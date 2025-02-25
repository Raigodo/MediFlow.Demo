using FluentValidation;
using MediFlow.Functions.Modules.Clients.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Clients;

public static class ClientsModuleExtensions
{
    public static IServiceCollection AddClientsModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClientCommand>, CreateClientCommandValidator>();
        services.AddScoped<IValidator<UpdateClientCommand>, UpdateClientCommandValidator>();
        return services;
    }
}
