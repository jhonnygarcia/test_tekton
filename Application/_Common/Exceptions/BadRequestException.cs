namespace Application._Common.Exceptions
{
    public class BadRequestException : BaseApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
        }
        public BadRequestException(string message, string[] details) : base(null, message, details)
        {
        }
        public BadRequestException(string[] details) : base(null, null, details) { }

    }
}
