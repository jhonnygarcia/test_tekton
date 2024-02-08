using Application._Common.Mapping;
using Application.ServicesClients.DiscountsProduct;
using Infraestructure;
using Infraestructure.ServicesClients.DiscountsProduct;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(expression =>
            {
                expression.AddProfile(new MappingProfile(typeof(DependencyInjection).Assembly));
            });

            services.AddHttpClient(InfraestructureConfiguration.DiscountConfig, (http) =>
            {
                http.BaseAddress = new Uri(configuration["ServicesClients:Discounts:BaseUrl"]);
            }).AddHttpMessageHandler<DiscountHttpMessageHandler>();
            services.AddTransient<DiscountHttpMessageHandler>();
            services.AddScoped<IDiscountsProductServiceClient, DiscountsProductServiceClient>();            
            return services;
        }
    }   
}
