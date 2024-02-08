using Application.Components.Security;
using Application.Components.Security.Models;
using Application.Components.Security.Settings;
using ITSystems.Framework.IComponents.Security;
using Microsoft.Extensions.Options;

namespace Infraestructure.Components.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IIdentitySessionService _identityService;
        private readonly SecurityOptions _config;
        public SecurityService(IIdentitySessionService identityService,
            IOptions<SecurityOptions> config)
        {
            _identityService = identityService;
            _config = config.Value;
        }
        public IdentitySessionInfo GetCurrentIdentityInfo()
        {
            return _identityService.GetUserIdentityInfo();
        }
        public async virtual Task<AuthenticationResult> LoginAsync(string login, string password)
        {
            var user = _config.BasicAuthentication.FirstOrDefault(x => x.UserName == login);
            if (user == null)
            {
                return new AuthenticationResult
                {
                    ResultType = AuthenticationResultType.NotFound
                };
            }

            if (user.Password != password)
            {
                return new AuthenticationResult
                {
                    ResultType = AuthenticationResultType.InvalidPassword
                };
            }

            var userInfo = new IdentitySessionInfo
            {
                Id = Guid.NewGuid().ToString(),
                Login = login,
                FirstName = "FirstName",
                Surname = "Surname",
                Email = $"{login}@email.com",
                IntegrationId = Guid.NewGuid().ToString(),
                MobileNumber = Guid.NewGuid().ToString(),
                Roles = [login]
            };

            return new AuthenticationResult
            {
                Identity = userInfo,
                ResultType = AuthenticationResultType.Success
            };
        }
        public async virtual Task<AuthenticationResult> GetIdentityInfoByTokenAsync(string token)
        {
            var result = await _identityService.GetUserIdentityInfoByTokenAsync(token);
            return result;
        }       
    }
}
