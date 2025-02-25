using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MediFlow.Api.Application.Auth.SchemeHandlers;

public class ExpiredTrinitySchemaHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthenticationService _authenticationService;

    public ExpiredTrinitySchemaHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IAuthenticationService authenticationService)
        : base(options, logger, encoder)
    {
        _authenticationService = authenticationService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.ExpiredUserAuthenticationSchema);
        if (!userResult.Succeeded)
        {
            return AuthenticateResult.Fail("User authentication failed.");
        }
        var userIdentity = userResult.Principal.Identity as ClaimsIdentity ?? throw new InvalidCastException();

        var deviceResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.DeviceIdSchema);
        if (!deviceResult.Succeeded && !userIdentity.Claims.Any(claims =>
                claims.Type == SessionClaims.UserRole
                &&
                (
                    claims.Value == UserRoles.Admin.ToString()
                    || claims.Value == UserRoles.Manager.ToString()
                ))
            )
        {
            return AuthenticateResult.Fail("Device authentication failed.");
        }


        var refreshResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.RefreshTokenSchema);
        if (!refreshResult.Succeeded)
        {
            return AuthenticateResult.Fail("Refresh token authentication failed.");
        }
        var refreshIdentity = refreshResult.Principal.Identity as ClaimsIdentity ?? throw new InvalidCastException();

        // Merge claims from both A1 and A2
        var identities = new List<ClaimsIdentity>
        {
            userIdentity,
            refreshIdentity,
        };

        if (deviceResult.Principal?.Identity as ClaimsIdentity is { } deviceIdentity)
        {
            identities.Add(deviceIdentity);
        }

        var principal = new ClaimsPrincipal(identities);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, AuthSchemas.ExpiredTrinitySchema));
    }
}
