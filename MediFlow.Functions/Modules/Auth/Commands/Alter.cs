using Mediator;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;
using MediFlow.Functions.Modules.Auth.Services.JwtProvider;
using MediFlow.Functions.Modules.Auth.Services.RefreshCookie;

namespace MediFlow.Functions.Modules.Auth.Commands;
using IResult = Result<AlterResponse, AuthErrors>;

public record AlterCommand(StructureId StructureId) : ICommand<IResult>;

public class AlterCommandHandler(
    ICurrentUserAccessor currentUser,
    IUserRepository userRepository,
    IStructureRepository structureRepository,
    ISessionRepository sessionRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider,
    IRefreshCookieAccessor refreshCookieAccessor,
    IAccessGuard accessGuard) : ICommandHandler<AlterCommand, IResult>
{
    public async ValueTask<IResult> Handle(AlterCommand command, CancellationToken cancellationToken)
    {
        var structure = await structureRepository.GetOneAsync(command.StructureId);
        if (structure is null)
        {
            return AuthErrors.StructureNotFound;
        }

        if (!await accessGuard.CanViewAsync(command.StructureId))
        {
            return AuthErrors.ForbidAccessToStructure;
        }

        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return AuthErrors.CurrentUserNotFound;
        }

        if (!refreshCookieAccessor.TryGetRefreshToken(out var refreshToken) || user.RefreshToken != refreshToken)
        {
            if (user.RefreshToken != refreshToken)
            {
                await refreshCookieAccessor.DeleteCookieAsync();
            }
            return AuthErrors.NoRefreshToken;
        }

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

        return new AlterResponse()
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
        };
    }
}
