using MediFlow.Functions.Data;
using MediFlow.Functions.Entities.Clients;
using MediFlow.Functions.Entities.Clients.Values;
using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Employees.Values;
using MediFlow.Functions.Entities.Notes;
using MediFlow.Functions.Entities.Notes.Values;
using MediFlow.Functions.Entities.Structures;
using MediFlow.Functions.Entities.Structures.Values;
using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared.Services.CurrentUserAccessor;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Functions.Modules._Shared.Services.AccessGuard;

public sealed class AccessGuard(
    AppDbContext dbContext,
    ICurrentUserAccessor currentUser) : IAccessGuard
{
    private IQueryable<Client> BuildQuery(ClientId clientId, GuardOptions guardOption)
    {
        //guardOption doesnt matter here, but included just for generalisation
        var query = dbContext.Clients.Where(c => c.Id == clientId);
        if (currentUser.UserRole == UserRoles.Admin) return query;
        return query.Where(c =>
                c.Structure.ManagerId == currentUser.UserId
                || c.Structure.Employees.Any(e => e.UserId == currentUser.UserId));
    }
    public Task<bool> ExistsAndCanAccessAsync(ClientId clientId, GuardOptions guardOption) =>
        BuildQuery(clientId, guardOption).AnyAsync();
    public Task<Client?> CheckAccessAndGetAsync(ClientId clientId, GuardOptions guardOption) =>
        BuildQuery(clientId, guardOption).FirstOrDefaultAsync();
    public Task<bool> CanCreateClientAsync(StructureId structureId) =>
        Task.FromResult(currentUser.UserRole >= UserRoles.Employee
            && currentUser.StructureId == structureId);


    private IQueryable<Note> BuildQuery(NoteId noteId, GuardOptions guardOption)
    {
        var query = dbContext.Notes.Where(n => n.Id == noteId);
        if (currentUser.UserRole == UserRoles.Admin)
            return query;

        if (guardOption == GuardOptions.Read)
        {
            query = query.Where(n =>
                n.Creator.Structure.ManagerId == currentUser.UserId
                || currentUser.EmployeeRole >= n.Creator.Role);
        }
        else
        {
            query = query.Where(n => n.Creator.UserId == currentUser.UserId);
        }
        return query;
    }
    public Task<bool> CanViewAsync(NoteId noteId)

    public Task<Note?> CheckAccessAndGetAsync(NoteId noteId, GuardOptions guardOption) =>
        BuildQuery(noteId, guardOption).FirstOrDefaultAsync();


    private IQueryable<Note> BuildQuery(ClientId clientId, EmployeeRoles employeeRole, GuardOptions guardOption)
    {
        var query = dbContext.Notes.Where(n =>
            n.Client.Id == clientId
            && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
            && n.Creator.Role == employeeRole);

        if (currentUser.UserRole == UserRoles.Admin)
            return query;

        if (guardOption == GuardOptions.Read)
        {
            return query.Where(n =>
                n.Creator.Structure.ManagerId == currentUser.UserId
                || currentUser.EmployeeRole >= n.Creator.Role);
        }
        return query.Where(n => n.Creator.UserId == currentUser.UserId);
    }
    public Task<bool> ExistsAndCanAccessAsync(ClientId clientId, EmployeeRoles employeeRole, GuardOptions guardOption) =>
        BuildQuery(clientId, employeeRole, guardOption).AnyAsync();
    public Task<Note?> CheckAccessAndGetAsync(ClientId clientId, EmployeeRoles employeeRole, GuardOptions guardOption) =>
        BuildQuery(clientId, employeeRole, guardOption).FirstOrDefaultAsync();
    public Task<bool> CanCreateNoteAsync(StructureId structureId) =>
        Task.FromResult(currentUser.UserRole == UserRoles.Employee
            && currentUser.StructureId == structureId);



    private IQueryable<Employee> BuildQuery(EmployeeId employeeId, GuardOptions guardOption)
    {
        var query = dbContext.Employees
            .Where(e => e.Id == employeeId);

        if (currentUser.EmployeeId == employeeId
            || currentUser.UserRole == UserRoles.Admin)
            return query;

        if (guardOption == GuardOptions.Read)
        {
            return query
                .Where(e =>
                    e.Structure.Employees.Any(e => e.UserId == currentUser.UserId)
                    || e.Structure.ManagerId == currentUser.UserId);
        }
        return query.Where(e => e.Structure.ManagerId == currentUser.UserId);
    }
    public Task<bool> ExistsAndCanAccessAsync(EmployeeId emplyeeId, GuardOptions guardOption) =>
        BuildQuery(emplyeeId, guardOption).AnyAsync();
    public Task<Employee?> CheckAccessAndGetAsync(EmployeeId emplyeeId, GuardOptions guardOption) =>
        BuildQuery(emplyeeId, guardOption).FirstOrDefaultAsync();


    private IQueryable<Invitation> BuildQuery(InvitationId invitationId, GuardOptions guardOption)
    {
        //guardOption doesnt matter here, but included just for generalisation
        var query = dbContext.Invitations
            .Where(i => i.Id == invitationId);

        if (currentUser.UserRole == UserRoles.Admin)
            return query;

        return query
            .Where(i => i.Structure.ManagerId == currentUser.UserId);
    }
    public Task<bool> ExistsAndCanAccessAsync(InvitationId invitationId, GuardOptions guardOption) =>
        BuildQuery(invitationId, guardOption).AnyAsync();
    public Task<Invitation?> CheckAccessAndGetAsync(InvitationId invitationId, GuardOptions guardOption) =>
        BuildQuery(invitationId, guardOption).FirstOrDefaultAsync();
    public Task<bool> CanCreateInvitationAsync(StructureId structureId) =>
        Task.FromResult(currentUser.UserRole == UserRoles.Manager
            && currentUser.StructureId == structureId);


    private IQueryable<Structure> BuildQuery(StructureId structureId, GuardOptions guardOption)
    {
        var query = dbContext.Structures
            .Where(s => s.Id == structureId);

        if (currentUser.UserRole == UserRoles.Admin)
            return query;

        if (guardOption == GuardOptions.Read)
        {
            return query.Where(s =>
                s.Employees.Any(e => e.UserId == currentUser.UserId)
                || s.ManagerId == currentUser.UserId);
        }
        return query.Where(s => s.ManagerId == currentUser.UserId);
    }
    public Task<bool> ExistsAndCanAccessAsync(StructureId structureId, GuardOptions guardOption) =>
        BuildQuery(structureId, guardOption).AnyAsync();
    public Task<Structure?> CheckAccessAndGetAsync(StructureId structureId, GuardOptions guardOption) =>
        BuildQuery(structureId, guardOption).FirstOrDefaultAsync();
    public Task<bool> CanCreateStructureAsync() =>
        Task.FromResult(currentUser.UserRole == UserRoles.Manager);


    private IQueryable<User> BuildQuery(UserId userId, GuardOptions guardOption)
    {
        var query = dbContext.Structures
            .Where(s =>
                //get structure where is current user
                s.Employees.Any(e => e.UserId == currentUser.UserId) || s.ManagerId == currentUser.UserId)
            .Where(s =>
                //get subset of structures where is second user
                s.Employees.Any(e => e.UserId == userId) || s.ManagerId == userId);

        if (currentUser.UserId != userId
            && currentUser.UserRole != UserRoles.Admin
            && guardOption != GuardOptions.Read)
        {
            query = query.Where(s => s.ManagerId == currentUser.UserId);
        }

        return query
            .SelectMany(s => s.Employees
                .Select(e => e.User)
                .Concat(new[] { s.Manager }));
    }
    public Task<bool> ExistsAndCanAccessAsync(UserId userId, GuardOptions guardOption) =>
        BuildQuery(userId, guardOption).AnyAsync();
    public Task<User?> CheckAccessAndGetAsync(UserId userId, GuardOptions guardOption) =>
        BuildQuery(userId, guardOption)
            .FirstOrDefaultAsync();
    public Task<bool> CanCreateUserAsync(UserId userId) =>
        Task.FromResult(currentUser.UserRole == UserRoles.Admin);
}