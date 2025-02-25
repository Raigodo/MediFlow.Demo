using Mediator;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Structures.Queries;
using Result = Result<Structure, AuthErrors>;

public record GetStructureQuery() : ICommand<Result>;

public class GetStructureQueryHandler() : ICommandHandler<GetStructureQuery, Result>
{
    public async ValueTask<Result> Handle(GetStructureQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
