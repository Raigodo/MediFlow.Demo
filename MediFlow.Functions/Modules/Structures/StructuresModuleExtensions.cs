using FluentValidation;
using MediFlow.Functions.Modules.Structures.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Structures;

public static class StructuresModuleExtensions
{
    public static IServiceCollection AddStructuresModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateStructureCommand>, CreateStructureCommandValidator>();
        services.AddScoped<IValidator<UpdateStructureCommand>, UpdateStructureCommandValidator>();
        return services;
    }
}
