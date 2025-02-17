namespace MediFlow.Api.Modules._Shared.Services.CurrentUserAccessor;

public sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    public SessionData CurrentUser { get; set; } = SessionData.Empty;
}
