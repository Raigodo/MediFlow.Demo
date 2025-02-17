using FluentValidation;
using MediFlow.Api.Modules.Structures.Endpoints;

namespace MediFlow.Api.Modules.Structures;

public static class StructuresModuleExtensions
{
    public static IServiceCollection AddStructuresModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateStructureRequest>, CreateStructureRequestValidator>();
        services.AddScoped<IValidator<UpdateStructureRequest>, UpdateStructureRequestValidator>();
        return services;
    }

    public static IEndpointRouteBuilder MapStructuresEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Structures")
            .MapGetStructureEndpoint()
            .MapCreateStructureEndpoint()
            .MapGetCurrentStructureEndpoint()
            .MapGetOwnedStructuresEndpoint()
            .MapGetParticipatingStructuresEndpoint()
            .MapUpdateStructureEndpoint()
            .MapRemoveStructureEndpoint()
            .MapTrustDeviceEndpoint();
        return routes;
    }
}
