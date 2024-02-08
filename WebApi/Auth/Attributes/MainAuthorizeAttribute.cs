using Application.Components.Security.Models;
using Application.Components.Security.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace WebApi.Auth.Attributes
{
    public class MainAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Authorize(context);

            return Task.CompletedTask;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Authorize(context);
        }

        protected void Authorize(AuthorizationFilterContext context)
        {
            var optionSecurity = context.HttpContext.RequestServices.GetService<IOptions<SecurityOptions>>();
            var config = optionSecurity.Value;

            if (!(context.HttpContext.User?.Identity?.IsAuthenticated ?? false))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var identityInfo = new IdentitySessionInfo();
            identityInfo.LoadClaims(context.HttpContext.User);

            var userRoles = identityInfo.Roles ?? new string[0];
            var schema = AuthenticationSchemes;            

            if (Roles != null &&
                !userRoles.Intersect(Roles.Split(',').Select(s => s.Trim())).Any())
            {
                context.Result = new ForbidResult(schema.Split(',')[0]);
                return;
            }
        }
    }
}
