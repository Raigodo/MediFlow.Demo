using Mediator;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Users.Commands;
using Result = Result<Unit, AuthErrors>;

public record RemoveUserCommand() : ICommand<Result>;

public class RemoveUserCommandHandler() : ICommandHandler<RemoveUserCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
