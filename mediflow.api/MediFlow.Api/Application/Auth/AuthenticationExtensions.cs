using MediFlow.Api.Application.Auth.SchemeHandlers;
using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Modules.Auth.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MediFlow.Api.Application.Auth;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var jwtOptions = services.BuildServiceProvider()
            .GetRequiredService<IOptions<JwtOptions>>()
            .Value;

        services.AddAuthentication(AuthSchemas.OptionalUserAndDevice)
            .AddScheme<AuthenticationSchemeOptions, OptionalUserAndDevicetSchemaHandler>(AuthSchemas.OptionalUserAndDevice, (_) => { })
            .AddScheme<AuthenticationSchemeOptions, UserAndDeviceSchemaHandler>(AuthSchemas.UserAndDeviceSchema, (_) => { })
            .AddScheme<AuthenticationSchemeOptions, TrinitySchemaHandler>(AuthSchemas.TrinitySchema, (_) => { })
            .AddScheme<AuthenticationSchemeOptions, ExpiredTrinitySchemaHandler>(AuthSchemas.ExpiredTrinitySchema, (_) => { })
            .AddJwtBearer(AuthSchemas.UserAuthenticationSchema, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return Task.CompletedTask;
                        }
                    };
                })
            .AddJwtBearer(AuthSchemas.ExpiredUserAuthenticationSchema, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie(AuthSchemas.RefreshTokenSchema, options =>
                {
                    options.Cookie.Name = "JWT-REFRESH-TOKEN";
                    //options.Cookie.HttpOnly = false;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.IsEssential = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookie.Path = "/";

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                })
            .AddCookie(AuthSchemas.DeviceIdSchema, options =>
                {
                    options.Cookie.Name = "DEVICE-TOKEN";
                    //options.Cookie.HttpOnly = false;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.IsEssential = true;
                    options.Cookie.Path = "/";
                    options.ExpireTimeSpan = TimeSpan.FromDays(365.25 * 10);

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                });

        return services;
    }
}