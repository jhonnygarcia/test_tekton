using Application.Components.Security.Models;
using Application.Components.Security.Settings;
using Azure.Core;
using ITSystems.Framework.IComponents.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Auth
{
    public class HttpIdentitySessionService : IIdentitySessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SecurityOptions _configuration;
        private readonly ILogger _logger;
        public HttpIdentitySessionService(IOptions<SecurityOptions> configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HttpIdentitySessionService> logger)
        {
            _configuration = configuration.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public IdentitySessionInfo GetUserIdentityInfo()
        {
            if (!(_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false)) return null;

            var identityInfo = new IdentitySessionInfo();
            identityInfo.LoadClaims(_httpContextAccessor.HttpContext.User);

            return identityInfo;
        }
        public async Task<AuthenticationResult> GetUserIdentityInfoByTokenAsync(string authToken)
        {
            try
            {
                var resultClaims = await GetClaimsFromTokenAsync(authToken);
                if (resultClaims == null) return null;

                var expire = resultClaims.Item1;
                var claims = resultClaims.Item2;

                var claimsIdentities = claims
                    .Select(claim =>
                    {
                        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.TryGetValue(claim.Type, out var claimType);
                        if (string.IsNullOrWhiteSpace(claimType))
                            return new ClaimsIdentity(new[]
                            {
                                new Claim(claim.Type, claim.Value)
                            });

                        return new ClaimsIdentity(new[]
                        {
                            new Claim(claimType, claim.Value)
                        });
                    }).ToArray();

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentities);
                var identityInfo = new IdentitySessionInfo();
                identityInfo.LoadClaims(claimsPrincipal);

                return new AuthenticationResult
                {
                    Identity = identityInfo,
                    Expiration = expire,
                    ResultType = AuthenticationResultType.Success
                };
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, "The token sent is corrupted");
                return null;
            }
        }
        private async Task<Tuple<DateTime, IEnumerable<Claim>>> GetClaimsFromTokenAsync(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.KeySecret))
            };

            var principal = await handler.ValidateTokenAsync(authToken, validationParameters);

            if (principal.SecurityToken == null ||
                principal.SecurityToken.ValidTo < DateTime.UtcNow) return null;

            return new Tuple<DateTime, IEnumerable<Claim>>(
                principal.SecurityToken.ValidTo, principal.ClaimsIdentity.Claims);
        }
        public virtual string GetAccessTokenFromRequest()
        {
            if (!(_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false)) return null;

            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].SingleOrDefault();
            if (authHeader == null || !authHeader.ToLower().StartsWith("bearer"))
            {
                return null;
            }
            return authHeader["bearer ".Length..].Trim();
        }
    }
}
