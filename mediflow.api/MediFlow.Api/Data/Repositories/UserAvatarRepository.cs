﻿using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;
using MediFlow.Api.Modules._Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Api.Data.Repositories;

public sealed class UserAvatarRepository(AppDbContext dbContext) : IUserAvatarRepository
{
    public void Add(UserAvatar avatarImage) => dbContext.UserAvatars.Add(avatarImage);
    public void Update(UserAvatar avatarImage) => dbContext.UserAvatars.Update(avatarImage);
    public void Delete(UserAvatar avatarImage) => dbContext.UserAvatars.Remove(avatarImage);

    public Task DeleteAsync(UserId userId) =>
        dbContext.UserAvatars
            .Where(i => i.UserId == userId)
            .ExecuteDeleteAsync();

    public Task<UserAvatar?> GetOneAsync(UserId userId) =>
        dbContext.UserAvatars
            .Where(i => i.UserId == userId)
            .FirstOrDefaultAsync();

    public Task<bool> ExistsAsync(UserId userId) =>
        dbContext.UserAvatars
            .Where(i => i.UserId == userId)
            .AnyAsync();
}
