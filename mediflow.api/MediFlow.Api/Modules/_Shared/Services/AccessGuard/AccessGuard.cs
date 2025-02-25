using MediFlow.Api.Data;
using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Entities.Structures.Values;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Modules._Shared.Services.AccessGuard;

public sealed class AccessGuard(
    AppDbContext dbContext,
    ICurrentUserAccessor currentUser) : IAccessGuard
{
    public Task<bool> CanViewAsync(ClientId clientId)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Clients
            .AnyAsync(c =>
                c.Id == clientId
                && (
                    c.Structure.ManagerId == currentUser.UserId
                    || c.Structure.Employees.Any(e => e.UserId == currentUser.UserId)
                )
            );
    }

    public Task<bool> CanWriteAsync(ClientId clientId) => CanViewAsync(clientId);


    public Task<bool> CanViewAsync(NoteId noteId)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Notes
            .AnyAsync(n =>
                n.Id == noteId
                &&
                (
                    n.Creator.Structure.ManagerId == currentUser.UserId
                    || currentUser.EmployeeRole >= n.Creator.Role
                )
            );
    }

    public Task<bool> CanWriteAsync(NoteId noteId)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Notes
            .AnyAsync(n =>
                n.Id == noteId
                && n.Creator.UserId == currentUser.UserId);
    }


    public Task<bool> CanViewAsync(ClientId clientId, EmployeeRoles employeeRole)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Notes
            .AnyAsync(n =>

                    n.Client.Id == clientId
                    && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
                    && n.Creator.Role == employeeRole

                &&
                (
                    n.Creator.Structure.ManagerId == currentUser.UserId
                    || currentUser.EmployeeRole >= n.Creator.Role
                )
            );
    }


    public Task<bool> CanWriteAsync(ClientId clientId, EmployeeRoles employeeRole)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Notes
            .AnyAsync(n =>

                    n.Client.Id == clientId
                    && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
                    && n.Creator.Role == employeeRole

                && n.Creator.UserId == currentUser.UserId
            );
    }


    public Task<bool> CanViewAsync(StructureId structureId)
    {
        if (currentUser.UserRole == UserRoles.Admin
            || currentUser.StructureId == structureId)
            return Task.FromResult(true);

        return dbContext.Structures
            .AnyAsync(s =>
                s.Id == structureId
                && (
                    s.Employees.Any(e => e.UserId == currentUser.UserId)
                    || s.ManagerId == currentUser.UserId
                )
            );
    }

    public Task<bool> CanWriteAsync(StructureId structureId)
    {
        if (currentUser.UserRole == UserRoles.Admin ||

                currentUser.UserRole == UserRoles.Manager
                && currentUser.StructureId == structureId
            )
            return Task.FromResult(true);

        return dbContext.StructureManagers
            .AnyAsync(s => s.StructureId == structureId && s.ManagerId == currentUser.UserId);
    }

    public Task<bool> CanViewAsync(InvitationId invitationId)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.Invitations
            .AnyAsync(s =>
                s.Id == invitationId
                && s.Structure.ManagerId == currentUser.UserId);
    }

    public Task<bool> CanWriteAsync(InvitationId invitationId) => CanViewAsync(invitationId);


    public Task<bool> CanViewAsync(EmployeeId employeeId)
    {
        if (currentUser.EmployeeId == employeeId
            || currentUser.UserRole == UserRoles.Admin)
            return Task.FromResult(true);

        return dbContext.Structures
            .AnyAsync(s =>
                (
                    s.Employees.Any(e => e.UserId == currentUser.UserId)
                    || s.ManagerId == currentUser.UserId
                )
                && s.Employees.Any(e => e.Id == employeeId));
    }

    public Task<bool> CanWriteAsync(EmployeeId employeeId)
    {
        if (currentUser.UserRole == UserRoles.Admin) return Task.FromResult(true);
        return dbContext.StructureManagers
            .AnyAsync(m =>
                m.ManagerId == currentUser.UserId
                && m.Structure.Employees.Any(e => e.Id == employeeId));
    }


    public Task<bool> CanViewAsync(UserId userId)
    {
        if (currentUser.UserId == userId || currentUser.UserRole == UserRoles.Admin)
            return Task.FromResult(true);
        return dbContext.Structures
            .Where(s =>
                //get structure where is current user
                s.Employees.Any(e => e.UserId == currentUser.UserId) || s.ManagerId == currentUser.UserId)
            .Where(s =>
                //get subset of structures where is second user
                s.Employees.Any(e => e.UserId == userId) || s.ManagerId == userId)
            .AnyAsync();
    }

    public Task<bool> CanWriteAsync(UserId userId)
    {
        return Task.FromResult(
            currentUser.UserRole == UserRoles.Admin
            || currentUser.UserId == userId);
    }
}
