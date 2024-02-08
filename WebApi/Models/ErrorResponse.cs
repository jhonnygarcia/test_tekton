namespace WebApi.Models
{
    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string[] Details { get; set; }
    }
}
