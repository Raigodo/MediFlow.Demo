using MediFlow.Api.Application.Auth.Values;
using Microsoft.AspNetCore.Authorization;

namespace MediFlow.Api.Application.Auth.Requirements;

public class RefreshTokenRequirement : IAuthorizationRequirement { }

public class RefreshTokenRequirementHandler : AuthorizationHandler<RefreshTokenRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RefreshTokenRequirement requirement)
    {
        var principal = context.User;
        var hasDeviceId = principal?.Identities
            .Any(i => i.IsAuthenticated && i.AuthenticationType == AuthSchemas.RefreshTokenSchema) ?? false;

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
