using Mediator;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Queries;
using Result = Result<ICollection<Structure>, AuthErrors>;

public record GetParticipatingStructuresQuery() : IQuery<Result>;

public class GetParticipatingStructuresCommandHandler() : IQueryHandler<GetParticipatingStructuresQuery, Result>
{
    public async ValueTask<Result> Handle(GetParticipatingStructuresQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
