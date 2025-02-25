using Mediator;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Commands;
using Result = Result<Structure, AuthErrors>;

public record UpdateStructureCommand() : ICommand<Result>;

public class UpdateStructureCommandHandler() : ICommandHandler<UpdateStructureCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateStructureCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
