using MediFlow.Api.Data.Services.DataEncryptor;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediFlow.Api.Modules._Shared.Services.TimeLimitedToken;

public sealed class TimeLimitedTokenHandler(IOptions<TimeLimitedTokenOptions> options, IDataEncryptor encryptor) : ITimeLimitedTokenHandler
{
    private readonly TimeLimitedTokenOptions _options = options.Value;


    public string GenerateTimeLimitedToken() => GenerateTimeLimitedToken([], TimeSpan.FromSeconds(60));
    public string GenerateTimeLimitedToken(Claim[] claims) => GenerateTimeLimitedToken(claims, TimeSpan.FromSeconds(60));
    public string GenerateTimeLimitedToken(TimeSpan expiresAfter) => GenerateTimeLimitedToken([], expiresAfter);

    public string GenerateTimeLimitedToken(Claim[] claims, TimeSpan expiresAfter)
    {
        var secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        var securityKey = new SymmetricSecurityKey(secretKeyBytes);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddMinutes(expiresAfter.TotalMinutes);
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims,
            expires: expirationDate
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return encryptor.Encrypt(jwtToken);
    }

    public bool ValidateAndExtract(string token, out Dictionary<string, string> claims)
    {
        try
        {
            token = encryptor.Decrypt<string>(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            var jwtToken = tokenHandler.ReadJwtToken(token);
            claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            return true;
        }
        catch
        {
            claims = [];
            return false;
        }
    }
}
