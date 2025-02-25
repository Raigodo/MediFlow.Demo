using Mediator;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Queries;
using Result = Result<ICollection<Structure>, AuthErrors>;

public record GetOwnedStructuresQuery() : IQuery<Result>;

public class GetOwnedStructuresQueryHandler() : IQueryHandler<GetOwnedStructuresQuery, Result>
{
    public async ValueTask<Result> Handle(GetOwnedStructuresQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
