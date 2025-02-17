using MediFlow.Api.Application.Auth.Values;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MediFlow.Api.Application.Auth.Handlers;

public class OptionalUserAndDevicetSchemaHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthenticationService _authenticationService;

    public OptionalUserAndDevicetSchemaHandler(
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
        var deviceResult = await _authenticationService.AuthenticateAsync(Context, AuthSchemas.DeviceIdSchema);

        var identities = new List<ClaimsIdentity>();


        if (userResult.Principal?.Identity as ClaimsIdentity is { } userIdentity)
        {
            identities.Add(userIdentity);
        }
        if (deviceResult.Principal?.Identity as ClaimsIdentity is { } deviceIdentity)
        {
            identities.Add(deviceIdentity);
        }

        var principal = new ClaimsPrincipal(identities);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, AuthSchemas.OptionalUserAndDevice));
    }
}
