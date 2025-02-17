namespace MediFlow.Api.Modules._Shared.Services.DeviceCookieAccessor
{
    public interface IDeviceCookieAccessor
    {
        Task DeleteCookieAsync();
        Task SetCookieAsync(Guid deviceToken);
        bool TryGetCookie(out Guid deviceToken);
    }
}