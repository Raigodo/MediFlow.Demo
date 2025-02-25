using FluentValidation;
using MediFlow.Functions.Modules.Invitations.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Invitations;

public static class InvitationsModuleExtensions
{
    public static IServiceCollection AddInvitationsModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateInvitationCommand>, CreateInvitationCommandValidator>();
        return services;
    }
}
