using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Users.Values;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MediFlow.Api.Application.Auth.SchemeHandlers;

public class UserAndDeviceSchemaHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthenticationService _authenticationService;

    public UserAndDeviceSchemaHandler(
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
        var userResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.UserAuthenticationSchema);
        if (!userResult.Succeeded)
        {
            return AuthenticateResult.Fail("User authentication failed.");
        }
        var userIdentity = userResult.Principal.Identity as ClaimsIdentity ?? throw new InvalidCastException();

        var deviceResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.DeviceIdSchema);
        if (!deviceResult.Succeeded && !userIdentity.Claims.Any(claims =>
                claims.Type == SessionClaims.UserRole
                && (
                    claims.Value == UserRoles.Admin.ToString()
                    || claims.Value == UserRoles.Manager.ToString()
                ))
            )
        {
            return AuthenticateResult.Fail("Device authentication failed.");
        }

        var identities = new ClaimsIdentity[]{
            userIdentity,
            deviceResult.Principal?.Identity as ClaimsIdentity ?? new(),
        };

        var principal = new ClaimsPrincipal(identities);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, AuthSchemas.UserAndDeviceSchema));
    }
}
