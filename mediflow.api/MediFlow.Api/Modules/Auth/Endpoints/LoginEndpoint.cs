using MediFlow.Api.Data.Services.PasswordHasher;
using MediFlow.Api.Data.Services.UnitOfWork;
using MediFlow.Api.Modules._Shared.Interfaces;
using MediFlow.Api.Modules._Shared.Services;
using MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor;
using MediFlow.Api.Modules.Auth.Services.JwtProvider;
using MediFlow.Api.Modules.Auth.Services.RefreshCookie;
using Serilog;

namespace MediFlow.Api.Modules.Auth.Endpoints;

public record LoginRequest(string Email, string Password);

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/login", Handle);
        return routes;
    }

    public static async Task<IResult> Handle(
        LoginRequest req,
        IUserRepository userRepository,
        IEmployeeRepository emploeeRepository,
        IStructureRepository structureRepository,
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher,
        IRefreshCookieAccessor refreshCookieAccessor,
        IDeviceCookieAccessor deviceCookieAccessor,
        ResponseFactory responseFactory)
    {
        Log.Information("hello there");

        throw new NotImplementedException();
    }
}
