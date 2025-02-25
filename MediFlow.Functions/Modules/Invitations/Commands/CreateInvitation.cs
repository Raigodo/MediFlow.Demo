using Mediator;
using MediFlow.Functions.Data.Services.UnitOfWork;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;

namespace MediFlow.Functions.Modules.Invitations.Commands;
using Result = Result<Invitation, InvitationsErrors>;

public record CreateInvitationCommand(StructureId StructureId, EmployeeRoles EmployeeRole) : ICommand<Result>;

public class CreateInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork,
    IAccessGuard accessGuard) : ICommandHandler<CreateInvitationCommand, Result>
{
    public async ValueTask<Result> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
    {
        var structure = await accessGuard.CheckAccessAndGetAsync(command.StructureId, GuardOptions.Create);
        if (structure is null)
        {
            return InvitationsErrors.UserCanNotAccessInvitations;
        }

        var invitation = new Invitation()
        {
            StructureId = command.StructureId,
            Role = command.EmployeeRole,
            Structure = structure,
        };
        invitationRepository.Add(invitation);

        await unitOfWork.SaveChangesAsync();

        return invitation;
    }
}
