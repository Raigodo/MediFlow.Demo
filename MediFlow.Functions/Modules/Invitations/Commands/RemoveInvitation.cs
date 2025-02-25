using Mediator;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Invitations.Commands;
using Result = Result<Unit, InvitationsErrors>;

public record RemoveInvitationCommand(InvitationId InvitationId) : ICommand<Result>;

public class RemoveInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IAccessGuard accessGuard) : ICommandHandler<RemoveInvitationCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveInvitationCommand command, CancellationToken cancellationToken)
    {
        var invitationExists = await invitationRepository.ExistsAsync(command.InvitationId);
        if (!invitationExists)
        {
            return InvitationsErrors.InvitationNotFound;
        }

        var hasAccess = await accessGuard.CanWriteAsync(command.InvitationId);
        if (!hasAccess)
        {
            return InvitationsErrors.ForbidAccessToInvitation;
        }

        await invitationRepository.DeleteAsync(command.InvitationId);

        return Unit.Value;
    }
}
