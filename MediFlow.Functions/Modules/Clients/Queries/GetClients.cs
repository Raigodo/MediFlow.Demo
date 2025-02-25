using Mediator;
using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Clients.Queries;
using Result = Result<ICollection<Client>, ClientsErrors>;

public record GetClientsQuery(StructureId StructureId) : ICommand<Result>;

public class GetClientsQueryHandler(
    IClientRepository clientRepository,
    IStructureRepository structureRepository,
    IAccessGuard accessGuard) : ICommandHandler<GetClientsQuery, Result>
{
    public async ValueTask<Result> Handle(GetClientsQuery command, CancellationToken cancellationToken)
    {
        var structureExists = await structureRepository.ExistsAsync(command.StructureId);
        if (!structureExists)
        {
            return ClientsErrors.StructureNotFound;
        }

        var hasAccess = await accessGuard.CanViewAsync(command.StructureId);
        if (!hasAccess)
        {
            return ClientsErrors.ForbidAccessToStructure;
        }

        var clients = await clientRepository.GetAllAsync(command.StructureId);

        return clients;
    }
}
