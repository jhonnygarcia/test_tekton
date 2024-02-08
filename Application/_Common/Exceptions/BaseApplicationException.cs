namespace Application._Common.Exceptions
{
    public abstract class BaseApplicationException : Exception
    {
        public string Code { get; protected set; }
        public string[] Details { get; protected set; }
        public BaseApplicationException(string message) : base(message) { }

        public BaseApplicationException(string code, string message = null)
            : base(message)
        {
            Code = code;
        }

        public BaseApplicationException(string code, string message, string[] details)
            : base(message)
        {
            Code = code;
            Details = details;
        }
    }
}
