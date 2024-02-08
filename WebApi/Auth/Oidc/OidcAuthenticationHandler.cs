using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Components.Security;
using Application.Components.Security.Models;
using ITSystems.Framework.IComponents.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace WebApi.Auth.Oidc
{
    public class OidcAuthenticationHandler : DefaultAuthenticationHandler<OidcAuthenticationOptions>
    {
        private const string TOKEN_CACHE_TEMPLATE = "token-account-identity-{0}";

        private readonly ISecurityService _securityService;
        private readonly IIdentitySessionService _identitySessionService;
        private readonly IMemoryCache _cache;

        public OidcAuthenticationHandler(IOptionsMonitor<OidcAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISecurityService securityService, IMemoryCache cache, 
            IIdentitySessionService identitySessionService) : base(options, logger, encoder)
        {
            _securityService = securityService;
            _cache = cache;
            _identitySessionService = identitySessionService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var accessToken = _identitySessionService.GetAccessTokenFromRequest();
            if (string.IsNullOrEmpty(accessToken))
            {
                return AuthenticateResult.Fail("No Token");
            }

            if (!_cache.TryGetValue<IdentitySessionInfo>(
                    string.Format(TOKEN_CACHE_TEMPLATE, accessToken),
                    out var userIdentity))
            {
                var result = await _securityService.GetIdentityInfoByTokenAsync(accessToken);
                if (result == null) return AuthenticateResult.Fail("Invalid Token");

                if (result.ResultType == AuthenticationResultType.Success && result.Identity != null)
                {
                    userIdentity = result.Identity;
                    if (result.Expiration.HasValue && DateTime.UtcNow.CompareTo(result.Expiration.Value) < 0)
                    {
                        _cache.Set(string.Format(TOKEN_CACHE_TEMPLATE, accessToken),
                            userIdentity, result.Expiration.Value);
                    }
                }
                else
                {
                    userIdentity = null;
                }
            }

            if (userIdentity == null)
            {
                return AuthenticateResult.Fail("Invalid Token");
            }

            return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(new List<ClaimsIdentity>
                    {
                        new(userIdentity.GetClaims(), Options.AuthenticationType)
                    }), Options.Scheme));
        }  
    }
}
