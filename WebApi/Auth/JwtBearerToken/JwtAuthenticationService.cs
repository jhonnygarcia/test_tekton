using Application.Components.Security;
using Application.Components.Security.Models;
using Application.Components.Security.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Auth.JwtBearerToken
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private const string BEARER_TOKEN_TYPE = "bearer";
        private const int DEFAULT_EXPIRE_TOKEN_IN_MINUTES = 60;
        private readonly ISecurityService _securityService;
        private SecurityOptions ConfigSecurity { get; }
        public JwtAuthenticationService(ISecurityService securityService,
            IOptions<SecurityOptions> securityModel)
        {
            _securityService = securityService;
            ConfigSecurity = securityModel.Value;
        }
        public async virtual Task<JwtTokenInfo> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var identify = await _securityService.LoginAsync(username, password);
            if (identify.ResultType != AuthenticationResultType.Success)
            {
                return null;
            }

            return GenerateTokenSecurity(identify.Identity,  ConfigSecurity.TokenExpireInMinutes);
        }
        public virtual JwtTokenInfo GenerateTokenSecurity(IdentitySessionInfo session, 
            int? tokenExpireInMinutes = null)
        {
            if (session == null) return null;
            var claims = session.GetClaims();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(ConfigSecurity.KeySecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = ConfigSecurity.Issuer,
                Audience = ConfigSecurity.Audience,
                Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpireInMinutes ??
                    ConfigSecurity.TokenExpireInMinutes ?? DEFAULT_EXPIRE_TOKEN_IN_MINUTES),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var expireIn = securityToken.ValidTo - securityToken.ValidFrom;
            return new JwtTokenInfo
            {
                TokenType = BEARER_TOKEN_TYPE,
                AccessToken = tokenHandler.WriteToken(securityToken),
                ExpireIn = Convert.ToInt32(expireIn.TotalSeconds)
            };
        }       
    }
}
