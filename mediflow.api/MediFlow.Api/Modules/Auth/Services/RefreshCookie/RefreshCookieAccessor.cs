using MediFlow.Api.Application.Auth.Values;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MediFlow.Api.Modules.Auth.Services.RefreshCookie;

public class RefreshCookieAccessor(IHttpContextAccessor httpContextAccessor) : IRefreshCookieAccessor
{
    public static readonly string ClaimName = "X-JWT-REFRESH-TOKEN-VALUE";

    public Task SetCookieAsync(Guid refreshToken)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new Exception("should never happen - request ran over non-http protocol");
        return httpContext.SignInAsync(
            AuthSchemas.RefreshTokenSchema,
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new Claim(ClaimName, refreshToken.ToString())
                    ],
                    AuthSchemas.RefreshTokenSchema
                )
            )
        );
    }

    public Task DeleteCookieAsync()
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new Exception("should never happen - request ran over non-http protocol");
        return httpContext.SignOutAsync(AuthSchemas.RefreshTokenSchema);
    }

    public bool TryGetRefreshToken(out Guid refreshToken)
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new Exception("should never happen - request ran over non-http protocol");

        refreshToken = Guid.Empty;

        var refreshTokenClaim = httpContext.User?.FindFirstValue(ClaimName);

        if (refreshTokenClaim != null && Guid.TryParse(refreshTokenClaim, out var token))
        {
            refreshToken = token;
            return true;
        }

        return false;
    }
}
