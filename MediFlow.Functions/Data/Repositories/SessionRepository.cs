using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;
using MediFlow.Functions.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Functions.Data.Repositories;

public sealed class SessionRepository(AppDbContext dbContext) : ISessionRepository
{
    public void Add(LastSession lastSession) => dbContext.LastSessions.Add(lastSession);
    public void Update(LastSession lastSession) => dbContext.LastSessions.Update(lastSession);
    public void Delete(LastSession lastSession) => dbContext.LastSessions.Remove(lastSession);

    public Task DeleteAsync(UserId userId) =>
        dbContext.LastSessions
            .Where(i => i.UserId == userId)
            .ExecuteDeleteAsync();

    public Task<LastSession?> GetOneAsync(UserId userId) =>
        dbContext.LastSessions
            .FirstOrDefaultAsync(s => s.UserId == userId);
}
