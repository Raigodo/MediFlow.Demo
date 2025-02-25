using Mediator;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Clients.Commands;
using Result = Result<Unit, ClientsErrors>;

public record RemoveClientCommand(ClientId ClientId) : ICommand<Result>;

public class RemoveClientCommandHandler(
        IClientRepository clientRepository,
        IAccessGuard accessGuard) : ICommandHandler<RemoveClientCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveClientCommand command, CancellationToken cancellationToken)
    {
        var canAccess = await accessGuard.ExistsAndCanAccessAsync(command.ClientId, GuardOptions.Delete);
        if (!canAccess)
        {
            return ClientsErrors.UserCanNotAccessClient;
        }

        await clientRepository.DeleteAsync(command.ClientId);

        return Unit.Value;
    }
}
