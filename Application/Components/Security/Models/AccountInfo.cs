namespace Application.Components.Security.Models
{
    public class AccountInfo
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }        
        public bool Active { get; set; }
        public string IntegrationId { get; set; }
    }
}
