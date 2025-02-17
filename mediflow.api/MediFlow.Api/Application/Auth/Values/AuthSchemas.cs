using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MediFlow.Api.Application.Auth.Values;

public static class AuthSchemas
{
    public const string UserAuthenticationSchema = JwtBearerDefaults.AuthenticationScheme;
    public const string ExpiredUserAuthenticationSchema = $"expired-user";
    public const string DeviceIdSchema = "device-id";
    public const string RefreshTokenSchema = "user-refresh-token";

    public const string UserAndDeviceSchema = "user-and-device";
    public const string TrinitySchema = "user-and-device-and-refresh";
    public const string ExpiredTrinitySchema = "expired-user-and-device-and-refresh";
    public const string OptionalUserAndDevice = "optional-user-and-sevice-scheme";
}
