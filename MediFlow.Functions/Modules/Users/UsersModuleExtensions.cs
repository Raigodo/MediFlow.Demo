using FluentValidation;
using MediFlow.Functions.Modules.Auth.Commands;
using MediFlow.Functions.Modules.Users.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Functions.Modules.Users;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        return services;
    }
}
