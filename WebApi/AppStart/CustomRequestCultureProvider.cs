using Microsoft.AspNetCore.Localization;

namespace WebApi.AppStart
{
    public class CustomRequestCultureProvider : RequestCultureProvider
    {
        private readonly ApiConfiguration _apiConfiguration;
        
        public CustomRequestCultureProvider(ApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;
        }

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var language = _apiConfiguration.DefaultCulture;
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            await Task.Yield();
            return new ProviderCultureResult(language);
        }
    }
}
