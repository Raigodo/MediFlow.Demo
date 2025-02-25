using Mediator;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Users.Commands;
using Result = Result<User, AuthErrors>;

public record UpdateUserCommand() : ICommand<Result>;

public class UpdateUserCommandHandler() : ICommandHandler<UpdateUserCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
