using System.Security.Claims;

namespace MediFlow.Api.Modules._Shared.Services.TimeLimitedToken
{
    public interface ITimeLimitedTokenHandler
    {
        string GenerateTimeLimitedToken();
        string GenerateTimeLimitedToken(Claim[] claims);
        string GenerateTimeLimitedToken(Claim[] claims, TimeSpan expiresAfter);
        string GenerateTimeLimitedToken(TimeSpan expiresAfter);
        bool ValidateAndExtract(string token, out Dictionary<string, string> claims);
    }
}