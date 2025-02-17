using MediFlow.Api.Entities.Clients.Values;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal;
using MediFlow.Api.Entities.Journal.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class NoteRepository(AppDbContext dbContext) : INoteRepository
{
    public void Add(Note manager) => dbContext.Notes.Add(manager);
    public void Update(Note manager) => dbContext.Notes.Update(manager);
    public void Delete(Note manager) => dbContext.Notes.Remove(manager);

    public Task DeleteAsync(ClientId clientId, EmployeeRoles employeeRole) =>
        dbContext.Notes
            .Where(n =>
                n.Client.Id == clientId
                && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
                && n.Creator.Role == employeeRole)
            .ExecuteDeleteAsync();

    public Task<bool> ExistsAsync(NoteId noteId) =>
        dbContext.Notes
            .AnyAsync(i => i.Id == noteId);

    public Task<bool> ExistsAsync(ClientId clientId, EmployeeRoles employeeRole) =>
        dbContext.Notes
            .AnyAsync(n =>
                n.Client.Id == clientId
                && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
                && n.Creator.Role == employeeRole);

    public Task<Note?> GetOneAsync(NoteId noteId) =>
        dbContext.Notes
            .Include(n => n.Creator)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(s => s.Id == noteId);

    public Task<Note?> GetOneAsync(ClientId clientId, EmployeeRoles employeeRole) =>
        dbContext.Notes
            .Include(n => n.Creator)
            .ThenInclude(c => c.User)
            .Where(n =>
                n.Client.Id == clientId
                && n.CreatedOn == DateOnly.FromDateTime(DateTime.UtcNow)
                && n.Creator.Role == employeeRole)
            .FirstOrDefaultAsync();

    public Task<Note[]> GetManyAsync(
        ClientId clientId,
        EmployeeRoles role,
        DateOnly? from,
        DateOnly? to,
        bool? isFlagged,
        int skipCount,
        int takeCount)
    {
        var query = dbContext.Notes
            .Include(n => n.Creator)
            .ThenInclude(c => c.User)
            .Where(s =>
                s.ClientId == clientId
                && s.Creator.Role == role);

        if (from is not null)
            query = query.Where(n => n.CreatedOn >= from);
        if (to is not null)
            query = query.Where(n => n.CreatedOn <= to);
        if (isFlagged is not null)
            query = query.Where(n => n.IsFlagged == isFlagged);

        return query
            .OrderByDescending(n => n.CreatedOn)
            .Skip(skipCount)
            .Take(takeCount)
            .ToArrayAsync();
    }
}
