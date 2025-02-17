
using MediFlow.Api.Application.Auth.Values;
using Microsoft.AspNetCore.Authorization;

namespace MediFlow.Api.Application.Auth.Requirements;

public class SessionRequirement : IAuthorizationRequirement { }

public class SessionRequirementHandler : AuthorizationHandler<SessionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionRequirement requirement)
    {
        var principal = context.User;
        var hasDeviceId = principal?.Identities
            .Any(i => i.IsAuthenticated && i.AuthenticationType == AuthSchemas.UserAuthenticationSchema) ?? false;

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
