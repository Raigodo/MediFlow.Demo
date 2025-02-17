using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

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
