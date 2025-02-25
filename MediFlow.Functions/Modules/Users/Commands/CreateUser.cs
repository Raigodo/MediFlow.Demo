using Mediator;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Users.Commands;
using Result = Result<User, AuthErrors>;

public record CreateUserCommand() : ICommand<Result>;

public class CreateUserCommandHandler() : ICommandHandler<CreateUserCommand, Result>
{
    public async ValueTask<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
