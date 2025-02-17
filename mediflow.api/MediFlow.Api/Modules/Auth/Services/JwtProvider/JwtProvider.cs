using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediFlow.Api.Modules.Auth.Services.JwtProvider;

public sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    public string GenerateAcessToken(
        UserId userId,
        UserRoles userRole,
        out DateTime expirationDate)
    {
        var userClaim = new Claim(SessionClaims.UserId, userId.ToString());
        var userRoleClaim = new Claim(SessionClaims.UserRole, userRole.ToString());
        return GenerateAcessToken([userClaim, userRoleClaim], out expirationDate);
    }

    public string GenerateAcessToken(
        UserId userId,
        UserRoles userRole,
        StructureId structureId,
        out DateTime expirationDate)
    {
        var userClaim = new Claim(SessionClaims.UserId, userId.ToString());
        var userRoleClaim = new Claim(SessionClaims.UserRole, userRole.ToString());
        var structureIdClaim = new Claim(SessionClaims.StructureId, structureId.ToString());
        return GenerateAcessToken([userClaim, userRoleClaim, structureIdClaim], out expirationDate);
    }

    public string GenerateAcessToken(
        UserId userId,
        UserRoles userRole,
        StructureId structureId,
        EmployeeRoles employeeRole,
        EmployeeId employeeId,
        out DateTime expirationDate)
    {
        var userClaim = new Claim(SessionClaims.UserId, userId.ToString());
        var structureClaim = new Claim(SessionClaims.StructureId, structureId.ToString());
        var employeeIdClaim = new Claim(SessionClaims.EmployeeId, employeeId.ToString());
        var employeeRoleClaim = new Claim(SessionClaims.EmployeeRole, employeeRole.ToString());
        var userRoleClaim = new Claim(SessionClaims.UserRole, userRole.ToString());
        return GenerateAcessToken([userClaim, structureClaim, employeeRoleClaim, userRoleClaim, employeeIdClaim], out expirationDate);
    }

    private string GenerateAcessToken(Claim[] claims, out DateTime expirationDate)
    {
        var secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        var securityKey = new SymmetricSecurityKey(secretKeyBytes);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        expirationDate = DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes);
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims,
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: expirationDate
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}