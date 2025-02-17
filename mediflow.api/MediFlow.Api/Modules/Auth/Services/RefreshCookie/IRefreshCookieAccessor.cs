namespace MediFlow.Api.Modules.Auth.Services.RefreshCookie
{
    public interface IRefreshCookieAccessor
    {
        Task DeleteCookieAsync();
        Task SetCookieAsync(Guid refreshToken);
        bool TryGetRefreshToken(out Guid refreshToken);
    }
}