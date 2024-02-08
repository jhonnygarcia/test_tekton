using Application.Components.Security.Models;
using System.Security.Claims;

namespace WebApi.Auth
{
    public static class IdentitySessionInfoExtensions
    {
        public const string CUSTOM_CLAIM_INTEGRATION_ID = "integration_id";
        public const string CUSTOM_CLAIM_MOBILE_PHONE = "mobilephone";

        public static List<Claim> GetClaims(this IdentitySessionInfo identity)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(identity.Id))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, identity.Id));
            }
            if (!string.IsNullOrEmpty(identity.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, identity.Email));
            };
            if (!string.IsNullOrEmpty(identity.Login))
            {
                claims.Add(new Claim(ClaimTypes.Upn, identity.Login));
            }
            if (!string.IsNullOrEmpty(identity.FirstName))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, identity.FirstName));
            }
            if (!string.IsNullOrEmpty(identity.Surname))
            {
                claims.Add(new Claim(ClaimTypes.Surname, identity.Surname));
            }
            if (!string.IsNullOrEmpty(identity.MobileNumber))
            {
                claims.Add(new Claim(CUSTOM_CLAIM_MOBILE_PHONE, identity.MobileNumber));
            }
            if (!string.IsNullOrWhiteSpace(identity.IntegrationId))
            {
                claims.Add(new Claim(CUSTOM_CLAIM_INTEGRATION_ID, identity.IntegrationId));
            }
            if (identity.Roles != null)
            {
                claims.AddRange(identity.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }     

            return claims;
        }

        public static void LoadClaims(this IdentitySessionInfo identity, ClaimsPrincipal claims)
        {
            identity.Id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            identity.FirstName = claims.FindFirst(ClaimTypes.GivenName)?.Value;
            identity.Surname = claims.FindFirst(ClaimTypes.Surname)?.Value;
            identity.Email = claims.FindFirst(ClaimTypes.Email)?.Value;
            identity.Login = claims.FindFirst(ClaimTypes.Upn)?.Value;
            identity.MobileNumber = claims.FindFirst(CUSTOM_CLAIM_MOBILE_PHONE)?.Value;
            identity.IntegrationId = claims.FindFirst(CUSTOM_CLAIM_INTEGRATION_ID)?.Value;
            identity.Roles = claims.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        }
    }
}
