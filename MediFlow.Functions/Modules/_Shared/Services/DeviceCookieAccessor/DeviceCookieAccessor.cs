using Microsoft.AspNetCore.Http;

namespace MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor;

public class DeviceCookieAccessor(IHttpContextAccessor httpContextAccessor) : IDeviceCookieAccessor
{
    public static readonly string ClaimName = "X-DEVICE-TOKEN-VALUE";

    public Task SetCookieAsync(Guid deviceToken)
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");
        //return httpContext.SignInAsync(
        //    AuthSchemas.DeviceIdSchema,
        //    new ClaimsPrincipal(
        //        new ClaimsIdentity(
        //            [
        //                new Claim(ClaimName, deviceToken.ToString())
        //            ],
        //            AuthSchemas.DeviceIdSchema
        //        )
        //    )
        //);
    }

    public Task DeleteCookieAsync()
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");
        //return httpContext.SignOutAsync(AuthSchemas.DeviceIdSchema);
    }

    public bool TryGetCookie(out Guid deviceToken)
    {
        throw new NotImplementedException();
        //var httpContext = httpContextAccessor.HttpContext
        //    ?? throw new Exception("should never happen - request ran over non-http protocol");

        //var isDeviceTokenSpecified = httpContext.User.FindFirstValue(ClaimName);
        //if (isDeviceTokenSpecified is not null && Guid.TryParse(isDeviceTokenSpecified, out deviceToken))
        //{
        //    return true;
        //}

        //deviceToken = default;
        //return false;
    }
}
