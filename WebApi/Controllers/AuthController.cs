using Application.Components.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Auth.Attributes;
using WebApi.Auth.JwtBearerToken;
using WebApi.Controllers.Base;
using WebApi.Models.RequestModel.Auth;

namespace WebApi.Controllers
{
    [ApiController]
    [Route(RoutePrefix)]
    public class AuthController : BaseController
    {
        public const string RoutePrefix = "api/auth";
        private readonly ISecurityService _securityService;
        private readonly IJwtAuthenticationService _jwtService;
        public AuthController(ISecurityService securityService,
             IJwtAuthenticationService jwtService)
        {
            _securityService = securityService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return CustomErrorResult(HttpStatusCode.BadRequest, "Tiene que enviar todos los parametros");
            }

            var token = await _jwtService.AuthenticateAsync(model.UserName, model.Password);
            if (token == null)
            {
                return CustomErrorResult(HttpStatusCode.Unauthorized, "Credenciales incorrectas");
            }

            return Ok(token);
        }

        [MainAuthorize]
        [HttpGet]
        [Route("userinfo")]
        public IActionResult UserInfo()
        {
            var userInfo = _securityService.GetCurrentIdentityInfo();
            return Ok(userInfo);
        }
    }
}
