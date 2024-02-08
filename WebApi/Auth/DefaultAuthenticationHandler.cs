using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WebApi.Auth
{
    public abstract class DefaultAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
        where TOptions : AuthenticationSchemeOptions, new()
    {
        private const string PROBLEM_DETAILS_CONTENT_TYPE = "application/problem+json";

        protected DefaultAuthenticationHandler(
            IOptionsMonitor<TOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = PROBLEM_DETAILS_CONTENT_TYPE;
            var problemDetails = new UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = PROBLEM_DETAILS_CONTENT_TYPE;
            var problemDetails = new ForbiddenProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
