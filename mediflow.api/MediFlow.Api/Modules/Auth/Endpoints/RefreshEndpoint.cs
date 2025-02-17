using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Api.Modules.Auth.Services.JwtProvider;
using MediFlow.Api.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefreshEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/refresh", Handle)
            .RequireAuthorization(AuthPolicies.SessionRefresh);
        return routes;
    }

    public static async Task<IResult> Handle(
        IUserRepository userRepository,
        IEmployeeRepository emploeeRepository,
        IStructureRepository structureRepository,
        ISessionRepository sessionRepository,
        ICurrentUserAccessor currentUser,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IRefreshCookieAccessor refreshCookieAccessor,
        IDeviceCookieAccessor deviceCookieAccessor,
        ResponseFactory responseFactory)
    {
        var hasDeviceKey = deviceCookieAccessor.TryGetCookie(out var deviceKey);

        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return responseFactory.Unauthorized();
        }

        if (!refreshCookieAccessor.TryGetRefreshToken(out var refreshToken))
        {
            if (user.RefreshToken != refreshToken)
            {
                await refreshCookieAccessor.DeleteCookieAsync();
            }
            return responseFactory.Unauthorized();
        }

        user.RefreshToken = Guid.NewGuid();


        var accessToken = string.Empty;
        var expirationDate = default(DateTime);
        var structureId = default(StructureId);
        var employeeId = default(EmployeeId);
        var employeeRole = EmployeeRoles.NotSpecified;


        if (user.Role == UserRoles.Employee)
        {
            var structureExists = await structureRepository.ExistsAsync(deviceKey);
            if (!structureExists)
            {
                await deviceCookieAccessor.DeleteCookieAsync();
                return responseFactory.Unauthorized();
            }

            var employee = await emploeeRepository.GetOneAsync(user.Id, deviceKey);

            if (employee is null)
            {
                return responseFactory.Unauthorized();
            }

            structureId = employee.StructureId;
            employeeId = employee.Id;
            employeeRole = employee.Role;

            accessToken = jwtProvider.GenerateAcessToken(
                user.Id,
                user.Role,
                structureId,
                employeeRole,
                employeeId,
                out expirationDate);
        }
        else if (user.Role == UserRoles.Manager || user.Role == UserRoles.Admin)
        {
            if (hasDeviceKey)
            {
                var structure = await structureRepository.GetOneAsync(deviceKey);
                if (structure is null)
                {
                    await deviceCookieAccessor.DeleteCookieAsync();
                }
                else
                {
                    structureId = structure.Id;
                }
            }
            if (structureId == default && user.Role == UserRoles.Manager)
            {
                var lastSession = await sessionRepository.GetOneAsync(user.Id);
                if (lastSession is not null)
                {
                    structureId = lastSession.StructureId;
                }
            }

            accessToken = structureId.IsEmpty()
                ? jwtProvider.GenerateAcessToken(user.Id, user.Role, out expirationDate)
                : jwtProvider.GenerateAcessToken(user.Id, user.Role, structureId, out expirationDate);
        }

        await refreshCookieAccessor.SetCookieAsync(user.RefreshToken);
        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(new RefreshResponse()
        {
            AccessToken = accessToken,
            ExpirationDate = expirationDate,
            SessionData = new()
            {
                UserId = user.Id,
                UserRole = user.Role,
                StructureId = structureId,
                EmployeeId = employeeId,
                EmployeeRole = employeeRole,
            }
        });
    }
}
