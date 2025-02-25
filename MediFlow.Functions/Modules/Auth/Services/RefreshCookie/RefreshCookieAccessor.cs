using Microsoft.AspNetCore.Http;

namespace MediFlow.Functions.Modules.Auth.Services.RefreshCookie;

public class RefreshCookieAccessor(IHttpContextAccessor httpContextAccessor) : IRefreshCookieAccessor
{
    public static readonly string ClaimName = "X-JWT-REFRESH-TOKEN-VALUE";

    public Task SetCookieAsync(Guid refreshToken)
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");
        //return httpContext.SignInAsync(
        //    AuthSchemas.RefreshTokenSchema,
        //    new ClaimsPrincipal(
        //        new ClaimsIdentity(
        //            [
        //                new Claim(ClaimName, refreshToken.ToString())
        //            ],
        //            AuthSchemas.RefreshTokenSchema
        //        )
        //    )
        //);
    }

    public Task DeleteCookieAsync()
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");
        //return httpContext.SignOutAsync(AuthSchemas.RefreshTokenSchema);
    }

    public bool TryGetRefreshToken(out Guid refreshToken)
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");

        //refreshToken = Guid.Empty;

        //var refreshTokenClaim = httpContext.User?.FindFirstValue(ClaimName);

        //if (refreshTokenClaim != null && Guid.TryParse(refreshTokenClaim, out var token))
        //{
        //    refreshToken = token;
        //    return true;
        //}

        //return false;
    }
}
