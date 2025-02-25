using Mediator;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules.Auth;

namespace MediFlow.Functions.Modules.Users.Queries;
using Result = Result<User, AuthErrors>;

public record GetUserQuery() : IQuery<Result>;

public class GetUserQueryHandler() : IQueryHandler<GetUserQuery, Result>
{
    public async ValueTask<Result> Handle(GetUserQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
