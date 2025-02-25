using Mediator;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Commands;
using Result = Result<Structure, AuthErrors>;

public record CreateStructureCommand() : ICommand<Result>;

public class CreateStructureCommandHandler() : ICommandHandler<CreateStructureCommand, Result>
{
    public async ValueTask<Result> Handle(CreateStructureCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
