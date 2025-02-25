using Mediator;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Users.Queries;
using Result = Result<ICollection<User>, AuthErrors>;

public record GetManagersQuery() : IQuery<Result>;

public class GetManagersQueryHandler() : IQueryHandler<GetManagersQuery, Result>
{
    public async ValueTask<Result> Handle(GetManagersQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
