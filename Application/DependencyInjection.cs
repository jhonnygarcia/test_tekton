using System.Reflection;
using Application._Common.Behaviours;
using Application._Common.Mapping;
using Application.Services;
using Application.Services.Impl;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(expression =>
            {
                expression.AddProfile(new MappingProfile(typeof(DependencyInjection).Assembly));
            });

            //Mediator
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
            // Register FLuentValidation
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // Services
            services.AddScoped<IProductStatusService, ProductStatusService>();

            return services;
        }
    }
}
