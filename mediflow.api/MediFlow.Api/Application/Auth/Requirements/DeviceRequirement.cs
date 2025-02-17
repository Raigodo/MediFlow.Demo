using MediFlow.Api.Application.Auth.Values;
using Microsoft.AspNetCore.Authorization;

namespace MediFlow.Api.Application.Auth.Requirements;

public class DeviceRequirement : IAuthorizationRequirement { }

public class DeviceRequirementHandler : AuthorizationHandler<DeviceRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeviceRequirement requirement)
    {
        var principal = context.User;
        var hasDeviceId = principal?.Identities
            .Any(i => i.IsAuthenticated && i.AuthenticationType == AuthSchemas.DeviceIdSchema) ?? false;

        if (hasDeviceId)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}
