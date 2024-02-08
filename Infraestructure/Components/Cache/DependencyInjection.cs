using Application.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Components.Cache
{ 
    public static class DependencyInjection
    {
        public static IServiceCollection AddCahceInMemory(this IServiceCollection services, IConfiguration configuration)
        {            
            services.AddSingleton<IDataCacheService, InMemoryDataCacheService>();
            return services;
        }
    }
}