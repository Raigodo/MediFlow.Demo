namespace MediFlow.Functions.Modules._Shared.Services.DeviceCookieAccessor
{
    public interface IDeviceCookieAccessor
    {
        Task DeleteCookieAsync();
        Task SetCookieAsync(Guid deviceToken);
        bool TryGetCookie(out Guid deviceToken);
    }
}