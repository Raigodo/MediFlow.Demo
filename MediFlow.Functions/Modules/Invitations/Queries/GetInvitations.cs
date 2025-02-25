using Mediator;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Modules._Shared;
using MediFlow.Functions.Modules._Shared.Interfaces;
using MediFlow.Functions.Modules._Shared.Services.AccessGuard;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;

namespace MediFlow.Functions.Modules.Invitations.Queries;
using Result = Result<ICollection<Invitation>, InvitationsErrors>;

public record GetInvitationsQuery() : IQuery<Result>;

public class GetInvitationsQueryHandler(
    IInvitationRepository invitationRepository,
    IStructureRepository structureRepository,
    ICurrentUserAccessor currentUser,
    IAccessGuard accessGuard) : IQueryHandler<GetInvitationsQuery, Result>
{
    public async ValueTask<Result> Handle(GetInvitationsQuery command, CancellationToken cancellationToken)
    {
        var structureExists = await structureRepository.ExistsAsync(currentUser.StructureId);
        if (!structureExists)
        {
            return InvitationsErrors.ForbidAccessToStructure;
        }

        var hasAccess = await accessGuard.CanViewAsync(currentUser.StructureId);

        var invitations = await invitationRepository.GetAllAsync(currentUser.StructureId);

        return invitations;
    }
}
