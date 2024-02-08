using Application.Components.Security.Models;

namespace ITSystems.Framework.IComponents.Security
{
    public interface IIdentitySessionService
    {
        IdentitySessionInfo GetUserIdentityInfo();
        Task<AuthenticationResult> GetUserIdentityInfoByTokenAsync(string authToken);
        string GetAccessTokenFromRequest();
    }
}
