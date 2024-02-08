using Application.Components.Security;
using Application.Components.Security.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Components.Security
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOwnSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SecurityOptions>(
                configuration.GetSection(SecurityOptions.Options));
            services.AddTransient<ISecurityService, SecurityService>();
            return services;
        }
    }
}
