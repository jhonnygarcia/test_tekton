namespace Application.Components.Security.Models
{
    public class IdentitySessionInfo
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string[] Roles { get; set; }
        public string MobileNumber { get; set; }
        public string IntegrationId { get; set; }
    }
}
