using Application.Components.Security.Models;

namespace WebApi.Auth.JwtBearerToken
{
    public interface IJwtAuthenticationService
    {
        Task<JwtTokenInfo> AuthenticateAsync(string username, string password);
        JwtTokenInfo GenerateTokenSecurity(IdentitySessionInfo Session, int? TokenExpireInMinutes = null);
    }
}
