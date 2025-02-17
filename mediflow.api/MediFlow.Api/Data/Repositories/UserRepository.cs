using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public void Add(User user) => dbContext.Users.Add(user);
    public void Update(User user) => dbContext.Users.Update(user);
    public void Delete(User user) => dbContext.Users.Remove(user);

    public Task DeleteAsync(UserId userId) =>
        dbContext.Users
            .Where(i => i.Id == userId)
            .ExecuteDeleteAsync();

    public Task<User?> GetOneAsync(UserId userId) =>
        dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    public Task<User?> GetOneAsync(string Email) =>
        dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == Email);

    public Task<bool> ExistsAsync(UserId userId) =>
        dbContext.Users
            .AnyAsync(u => u.Id == userId);

    public Task<bool> ExistsAsync(string Email) =>
        dbContext.Users
            .AnyAsync(u => u.Email == Email);

    public Task<User[]> GetManyAsync(UserRoles userRole) =>
        dbContext.Users
            .Where(u => u.Role == userRole)
            .ToArrayAsync();

    public Task<User[]> GetManyAsync() => dbContext.Users.ToArrayAsync();
}
