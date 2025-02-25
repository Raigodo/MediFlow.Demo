using Mediator;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Commands;
using Result = Result<Unit, AuthErrors>;

public record RemoveStructureCommand() : ICommand<Result>;

public class RemoveStructureCommandHandler() : ICommandHandler<RemoveStructureCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveStructureCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
