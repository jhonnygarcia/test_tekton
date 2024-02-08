namespace Application._Common.Exceptions
{
    public class BusinessException : BaseApplicationException
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
