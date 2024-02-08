using Application._Common.Mapping;
using Application.Constants;
using ITSystems.Framework.IComponents.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.AppStart;
using WebApi.Auth;
using WebApi.Auth.JwtBearerToken;
using WebApi.Auth.Oidc;
using WebApi.Auth.Policies;

namespace WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(expression =>
            {
                expression.AddProfile(new MappingProfile(typeof(DependencyInjection).Assembly));
            });

            services.AddSingleton<ApiConfiguration>();
            services.AddTransient<IIdentitySessionService, HttpIdentitySessionService>();
            services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();

            services.AddMemoryCache();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeToUTCConverter());
            });
            services.AddOptions();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization()
                     .AddJsonOptions(options =>
                     {
                         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                         options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                     });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddMainPolicies();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Security:Issuer"],
                    ValidAudience = configuration["Security:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:KeySecret"]))
                };
            })
            .AddScheme<OidcAuthenticationOptions, OidcAuthenticationHandler>(OidcAuthenticationOptions.DEFAULT_SCHEME, (_) => { });

            ConfigureExtraServices(services, configuration);
            return services;
        }
        private static void ConfigureExtraServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });
            ConfigureSwagger(services, configuration);
        }
        private static void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
        {
            var appName = configuration["AppSettings:AppName"];

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = appName, Version = "v1" });
                c.EnableAnnotations();
                // Config Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Add a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}