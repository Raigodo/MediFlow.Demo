using Mediator;
using MediFlow.Functions.Entities.Notes;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Notes.Commands;
using Result = Result<Note, AuthErrors>;

public record WriteNoteCommand() : ICommand<Result>;

public class WriteNoteCommandHandler() : ICommandHandler<WriteNoteCommand, Result>
{
    public async ValueTask<Result> Handle(WriteNoteCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
