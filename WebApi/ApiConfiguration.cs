namespace WebApi
{
    public class ApiConfiguration
    {
        public const string GLOBAL_ENV_VAR = "ASPNETCORE_ENVIRONMENT";
        public const string PATH_RESOURCES = "Resources";
        public const string DEFAULT_CULTURE = "en-US";

        private readonly IConfiguration _configuration;
        public ApiConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }        
        public virtual string DefaultCulture => _configuration["AppSettings:Culture"] ?? DEFAULT_CULTURE;
    }
}
