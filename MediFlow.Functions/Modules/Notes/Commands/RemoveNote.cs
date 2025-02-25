using Mediator;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Notes.Commands;
using Result = Result<Unit, AuthErrors>;

public record RemoveNoteCommand() : ICommand<Result>;

public class RemoveNoteCommandHandler() : ICommandHandler<RemoveNoteCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveNoteCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
