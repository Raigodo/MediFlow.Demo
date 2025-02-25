using MediFlow.Functions.Entities.Users;
using MediFlow.Functions.Entities.Users.Values;

namespace MediFlow.Functions.Modules._Shared.Interfaces
{
    public interface ISessionRepository
    {
        void Add(LastSession lastSession);
        void Delete(LastSession lastSession);
        Task DeleteAsync(UserId userId);
        Task<LastSession?> GetOneAsync(UserId userId);
        void Update(LastSession lastSession);
    }
}