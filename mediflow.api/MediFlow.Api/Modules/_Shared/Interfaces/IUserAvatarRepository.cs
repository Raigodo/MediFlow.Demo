using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
{
    public interface IUserAvatarRepository
    {
        void Add(UserAvatar avatarImage);
        void Delete(UserAvatar avatarImage);
        Task DeleteAsync(UserId userId);
        Task<bool> ExistsAsync(UserId userId);
        Task<UserAvatar?> GetOneAsync(UserId userId);
        void Update(UserAvatar avatarImage);
    }
}