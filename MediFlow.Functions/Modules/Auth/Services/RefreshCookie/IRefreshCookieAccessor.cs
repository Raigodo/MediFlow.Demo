namespace MediFlow.Functions.Modules.Auth.Services.RefreshCookie
{
    public interface IRefreshCookieAccessor
    {
        Task DeleteCookieAsync();
        Task SetCookieAsync(Guid refreshToken);
        bool TryGetRefreshToken(out Guid refreshToken);
    }
}