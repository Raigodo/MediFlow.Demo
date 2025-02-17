using MediFlow.Api.Entities.Users;
using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Modules._Shared.Interfaces
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