using Microsoft.AspNetCore.Authentication;

namespace WebApi.Auth.Oidc
{
    public class OidcAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DEFAULT_SCHEME = "MetaFor Bearer Token";
        public string Scheme => DEFAULT_SCHEME;
        public string AuthenticationType = DEFAULT_SCHEME;
    }
}
