using Mediator;
using MediFlow.Functions.Entities.Notes;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Notes.Queries;
using Result = Result<IList<Note>, AuthErrors>;

public record GetNotesQuery() : IQuery<Result>;

public class GetNotesQueryHandler() : IQueryHandler<GetNotesQuery, Result>
{
    public async ValueTask<Result> Handle(GetNotesQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
