using Application.Main;
using DataAccess.Main.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainContext>(o =>
            o.UseSqlServer(configuration.GetConnectionString("MainConnection")));
            services.AddScoped<IMainContext>(provider => provider.GetService<MainContext>());
            return services;
        }
    }
}