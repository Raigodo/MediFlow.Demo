using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        void Delete(User user);
        Task DeleteAsync(UserId userId);
        Task<bool> ExistsAsync(string Email);
        Task<bool> ExistsAsync(UserId userId);
        Task<User[]> GetManyAsync();
        Task<User[]> GetManyAsync(UserRoles userRole);
        Task<User?> GetOneAsync(string Email);
        Task<User?> GetOneAsync(UserId userId);
        void Update(User user);
    }
}