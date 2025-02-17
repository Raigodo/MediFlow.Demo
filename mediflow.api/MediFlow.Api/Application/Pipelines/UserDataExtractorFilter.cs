using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MediFlow.Api.Application.Pipelines;


public sealed class UserDataExtractorFilter(
    [FromKeyedServices("internal")] CurrentUserAccessor currentUserAccessor
    ) : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        if (httpContext.User.Identity is { } identity)
        {
            if (identity.IsAuthenticated)
            {
                var principal = httpContext.User;

                _ = Guid.TryParse(principal.FindFirstValue(SessionClaims.UserId), out var userId);
                _ = Guid.TryParse(principal.FindFirstValue(SessionClaims.StructureId), out var structureId);
                _ = Guid.TryParse(principal.FindFirstValue(SessionClaims.EmployeeId), out var employeeId);
                _ = Enum.TryParse<UserRoles>(principal.FindFirstValue(SessionClaims.UserRole), out var userRole);
                _ = Enum.TryParse<EmployeeRoles>(principal.FindFirstValue(SessionClaims.EmployeeRole), true, out var employeeRole);

                currentUserAccessor.CurrentUser = new(
                    UserId.Create(userId),
                    StructureId.Create(structureId),
                    employeeRole,
                    userRole,
                    EmployeeId.Create(employeeId)
                );
            }
            else
            {
                if (httpContext.Request.Headers.TryGetValue("Authorization", out var headerValue))
                {
                    var token = headerValue.ToString().Split(" ").Last();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    var userClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == SessionClaims.UserId)?.Value;
                    var structureClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == SessionClaims.StructureId)?.Value;
                    var employeeIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == SessionClaims.EmployeeId)?.Value;
                    var employeeRoleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == SessionClaims.EmployeeRole)?.Value;
                    var userRoleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == SessionClaims.UserRole)?.Value;

                    _ = Guid.TryParse(userClaim, out var userId);
                    _ = Guid.TryParse(structureClaim, out var structureId);
                    _ = Guid.TryParse(employeeIdClaim, out var employeeId);
                    _ = Enum.TryParse<UserRoles>(employeeRoleClaim, true, out var userRole);
                    _ = Enum.TryParse<EmployeeRoles>(employeeRoleClaim, true, out var employeeRole);

                    currentUserAccessor.CurrentUser = new(
                        UserId.Create(userId),
                        StructureId.Create(structureId),
                        employeeRole,
                        userRole,
                        EmployeeId.Create(employeeId)
                    );
                }
            }
        }
        return next(context);
    }
}