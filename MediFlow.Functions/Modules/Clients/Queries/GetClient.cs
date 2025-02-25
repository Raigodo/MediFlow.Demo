using Mediator;
using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Clients.Queries;
using Result = Result<Client, ClientsErrors>;

public record GetClientQuery(ClientId ClientId) : IQuery<Result>;

public class GetClientQueryHandler(
    IClientRepository clientRepository,
    IAccessGuard accessGuard) : IQueryHandler<GetClientQuery, Result>
{
    public async ValueTask<Result> Handle(GetClientQuery command, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetOneAsync(command.ClientId);
        if (client is null)
        {
            return ClientsErrors.ClientNotFound;
        }

        var hasAccess = await accessGuard.CanViewAsync(command.ClientId);
        if (!hasAccess)
        {
            return ClientsErrors.ForbidAccessToClient;
        }

        return client;
    }
}
