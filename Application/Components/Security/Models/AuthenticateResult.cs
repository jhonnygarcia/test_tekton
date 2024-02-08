namespace Application.Components.Security.Models
{
    public enum AuthenticationResultType
    {
        NotFound,
        Locked,
        InvalidPassword,
        Success
    }
    public class AuthenticationResult
    {
        public IdentitySessionInfo Identity { get; set; }
        public DateTime? Expiration { get; set; }
        public AuthenticationResultType ResultType { get; set; }
    }
}
