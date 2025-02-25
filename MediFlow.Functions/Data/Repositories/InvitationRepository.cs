using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Functions.Data.Repositories;

public sealed class InvitationRepository(AppDbContext dbContext) : IInvitationRepository
{
    public void Add(Invitation invitation) => dbContext.Invitations.Add(invitation);
    public void Update(Invitation invitation) => dbContext.Invitations.Update(invitation);
    public void Delete(Invitation invitation) => dbContext.Invitations.Remove(invitation);

    public Task DeleteAsync(InvitationId invitationId) =>
        dbContext.Invitations
            .Where(i => i.Id == invitationId)
            .ExecuteDeleteAsync();

    public Task<Invitation?> GetOneAsync(InvitationId invitationId) =>
        dbContext.Invitations
            .Include(i => i.Structure)
            .FirstOrDefaultAsync(i => i.Id == invitationId);
    public Task<bool> ExistsAsync(InvitationId invitationId) =>
        dbContext.Invitations
            .AnyAsync(i => i.Id == invitationId);

    public Task<Invitation[]> GetAllAsync(StructureId structureId) =>
        dbContext.Invitations
            .Include(i => i.Structure)
            .Where(i => i.StructureId == structureId)
            .ToArrayAsync();
}
