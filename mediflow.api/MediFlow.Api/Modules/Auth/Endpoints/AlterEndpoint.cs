using MediFlow.Api.Application.Auth.Values;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Structures;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.AccessGuard;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Api.Modules.Auth.Services.JwtProvider;
using MediFlow.Api.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record AlterRequest(StructureId StructureId);

public static class AlterEndpoint
{
    public static IEndpointRouteBuilder MapAlterEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/alter", Handle)
            .RequireAuthorization(AuthPolicies.SessionMutation);
        return routes;
    }

    public static async Task<IResult> Handle(
        AlterRequest req,
        IUserRepository userRepository,
        IStructureRepository structureRepository,
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        ICurrentUserAccessor currentUser,
        IRefreshCookieAccessor refreshCookieAccessor,
        IAccessGuard accessGuard,
        ResponseFactory responseFactory)
    {
        if (!await accessGuard.CanViewAsync(req.StructureId))
        {
            return responseFactory.Unauthorized();
        }

        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return responseFactory.Unauthorized();
        }

        if (!refreshCookieAccessor.TryGetRefreshToken(out var refreshToken) || user.RefreshToken != refreshToken)
        {
            if (user.RefreshToken != refreshToken)
            {
                await refreshCookieAccessor.DeleteCookieAsync();
            }
            return responseFactory.Unauthorized();
        }

        var structure = await structureRepository.GetOneAsync(req.StructureId);

        if (structure is null)
        {
            return responseFactory.NotFound<Structure>();
        }
        //CHECKPOINT: checks passed, can proceed

        user.RefreshToken = Guid.NewGuid();

        var accessToken = jwtProvider.GenerateAcessToken(
            user.Id,
            user.Role,
            structure.Id,
            out var expirationDate);

        var lastSession = await sessionRepository.GetOneAsync(currentUser.UserId);

        if (lastSession is null)
        {
            lastSession = new LastSession()
            {
                UserId = currentUser.UserId,
                StructureId = structure.Id,
            };
            sessionRepository.Add(lastSession);
        }
        else
        {
            lastSession.StructureId = structure.Id;
        }

        await unitOfWork.SaveChangesAsync();

        await refreshCookieAccessor.SetCookieAsync(user.RefreshToken);

        return responseFactory.Ok(new AlterResponse()
        {
            AccessToken = accessToken,
            ExpirationDate = expirationDate,
            SessionData = new()
            {
                UserId = user.Id,
                UserRole = user.Role,
                StructureId = structure?.Id ?? StructureId.Create(Guid.Empty),
                EmployeeRole = EmployeeRoles.NotSpecified,
                EmployeeId = EmployeeId.Create(Guid.Empty),
            }
        });
    }
}
