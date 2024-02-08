using Application;
using DataAccess;
using Infrastructure;
using Serilog;
using WebApi;
using WebApi.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using Infraestructure.Components.Cache;
using Infraestructure.Components.Security;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

builder.Logging.AddSerilog(logger);

var environment = Environment.GetEnvironmentVariable(ApiConfiguration.GLOBAL_ENV_VAR);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddPresentation(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOwnSecurity(builder.Configuration);
builder.Services.AddCahceInMemory(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var appName = builder.Configuration["AppSettings:AppName"];
        c.SwaggerEndpoint("v1/swagger.json", appName);
        c.DocumentTitle = $"Swagger {appName}";
        c.DocExpansion(DocExpansion.None);
    });
}

app.UseMiddleware<CustomErrorHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Globalization and Localization Support
var apiConfiguration = app.Services.GetRequiredService<ApiConfiguration>();
var languagesSuported = new[] { "es-ES", "en-US" };
var localizaoptions = new RequestLocalizationOptions()
    .SetDefaultCulture(apiConfiguration.DefaultCulture)
    .AddSupportedCultures(languagesSuported)
    .AddSupportedUICultures(languagesSuported);

localizaoptions.RequestCultureProviders.Clear();
localizaoptions.RequestCultureProviders.Add(new WebApi.AppStart.CustomRequestCultureProvider(apiConfiguration));
app.UseRequestLocalization(localizaoptions);

app.Run();
