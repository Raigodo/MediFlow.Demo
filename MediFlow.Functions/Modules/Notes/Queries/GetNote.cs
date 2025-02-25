using Mediator;
using MediFlow.Functions.Entities.Notes;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Notes.Queries;
using Result = Result<Note, AuthErrors>;

public record GetNoteQuery() : IQuery<Result>;

public class GetNoteCommandHandler() : IQueryHandler<GetNoteQuery, Result>
{
    public async ValueTask<Result> Handle(GetNoteQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
