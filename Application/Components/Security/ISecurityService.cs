using Application.Components.Security.Models;

namespace Application.Components.Security
{
    public interface ISecurityService
    {
        IdentitySessionInfo GetCurrentIdentityInfo();
        Task<AuthenticationResult> LoginAsync(string login, string password);
        Task<AuthenticationResult> GetIdentityInfoByTokenAsync(string token);
    }
}
