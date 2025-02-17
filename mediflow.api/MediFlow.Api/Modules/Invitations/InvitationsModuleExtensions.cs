using FluentValidation;
using MediFlow.Api.Modules.Invitations.Endpoints;

namespace MediFlow.Api.Modules.Invitations;

public static class InvitationsModuleExtensions
{
    public static IServiceCollection AddInvitationsModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateInvitationRequest>, CreateInvitationRequestValidator>();
        return services;
    }

    public static IEndpointRouteBuilder MapInvitationsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/")
            .WithTags("Invitations")
            .MapGetInvitationsEndpoint()
            .MapCreateInvitationEndpoint()
            .MapRemoveInvitationEndpoint();
        return routes;
    }
}
