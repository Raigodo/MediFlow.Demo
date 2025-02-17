using MediFlow.Api.Application.Auth.Requirements;
using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.AspNetCore.Authorization;

namespace MediFlow.Api.Application.Auth;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, DeviceWeakRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, DeviceRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, RefreshTokenRequirementHandler>();

        services.AddAuthorization(options =>
        {
            var requireDeviceKeyOrManagerPlus = new DeviceOrManagerPlusRequirement();
            var deviceRequirement = new DeviceRequirement();
            var requireRefreshToken = new RefreshTokenRequirement();

            options.AddPolicy(AuthPolicies.Device, policy =>
                policy
                    .RequireAuthenticatedUser()
                    .AddRequirements(deviceRequirement));

            options.AddPolicy(AuthPolicies.SessionRefresh, policy =>
                policy
                    .AddAuthenticationSchemes(AuthSchemas.ExpiredTrinitySchema)
                    .RequireAuthenticatedUser()
                    .AddRequirements(requireDeviceKeyOrManagerPlus)
                    .AddRequirements(requireRefreshToken));

            options.AddPolicy(AuthPolicies.SessionMutation, policy =>
                policy
                    .AddAuthenticationSchemes(AuthSchemas.TrinitySchema)
                    .RequireAuthenticatedUser()
                    .AddRequirements(requireDeviceKeyOrManagerPlus)
                    .AddRequirements(requireRefreshToken));

            options.AddPolicy(AuthPolicies.OnlyAdmin, policy =>
                policy
                    .AddAuthenticationSchemes(AuthSchemas.UserAuthenticationSchema)
                    .RequireAuthenticatedUser()
                    .RequireClaim(SessionClaims.UserRole, UserRoles.Admin.ToString()));

            string[] managerPlusRoles = [
                UserRoles.Admin.ToString(),
                UserRoles.Manager.ToString()
            ];
            options.AddPolicy(AuthPolicies.ManagerPlus, policy =>
                policy
                    .AddAuthenticationSchemes(AuthSchemas.UserAndDeviceSchema)
                    .RequireAuthenticatedUser()
                    .AddRequirements(requireDeviceKeyOrManagerPlus)
                    .RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            c.Type == SessionClaims.UserRole
                            && managerPlusRoles.Contains(c.Value))));

            options.AddPolicy(AuthPolicies.EmployeePlus, policy =>
                policy
                    .AddAuthenticationSchemes(AuthSchemas.UserAndDeviceSchema)
                    .RequireAuthenticatedUser()
                    .AddRequirements(requireDeviceKeyOrManagerPlus));

            options.DefaultPolicy = options.GetPolicy(AuthPolicies.EmployeePlus)!;
        });

        return services;
    }
}