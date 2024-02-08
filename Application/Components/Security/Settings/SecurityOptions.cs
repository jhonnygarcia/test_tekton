namespace Application.Components.Security.Settings
{
    public class SecurityOptions
    {
        public const string Options = "Security";

        public string KeySecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int? TokenExpireInMinutes { get; set; }
        public UserAuthentication[] BasicAuthentication { get; set; }
    }
    public class UserAuthentication
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
