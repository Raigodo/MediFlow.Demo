using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MediFlow.Api.Application.Auth.Requirements;

public class DeviceOrManagerPlusRequirement : IAuthorizationRequirement { }

public class DeviceWeakRequirementHandler : AuthorizationHandler<DeviceOrManagerPlusRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeviceOrManagerPlusRequirement requirement)
    {
        var principal = context.User;
        var hasDeviceId = principal?.Identities
            .Any(i => i.IsAuthenticated && i.AuthenticationType == AuthSchemas.DeviceIdSchema) ?? false;
        var isManager = principal is not null ? IsManagerPlus(principal) : false;

        if (principal is not null && (hasDeviceId || isManager))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }

    private static bool IsManagerPlus(ClaimsPrincipal principal)
    {
        var isManagerPlus = principal?.Identities.Any(identity =>
            identity.Claims.Any(claims =>
                claims.Type == SessionClaims.UserRole
                && (claims.Value == UserRoles.Admin.ToString()
                    || claims.Value == UserRoles.Manager.ToString())
                )
            ) ?? false;

        return isManagerPlus;
    }
}
