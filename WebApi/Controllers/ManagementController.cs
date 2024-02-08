using Application.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApi.Auth.Attributes;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    [ApiController]
    [Route(RoutePrefix)]
    [MainAuthorize(Roles = SystemRoles.Admin)]
    public class ManagementController : BaseController
    {
        public const string RoutePrefix = "api/management";

        [Route("environment")]
        [HttpGet]
        public ActionResult GetCurrentEnvironment()
        {
            var env = Environment.GetEnvironmentVariable(ApiConfiguration.GLOBAL_ENV_VAR);
            return Ok(env);
        }

        [Route("~/version")]
        [HttpGet]
        public ActionResult GetVersion()
        {            
            var assembly = GetType().Assembly; 
            var versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return Ok(versionAttribute.InformationalVersion);
        }
    }
}
