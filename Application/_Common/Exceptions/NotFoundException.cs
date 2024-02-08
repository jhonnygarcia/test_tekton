namespace Application._Common.Exceptions
{
    public class NotFoundException : BaseApplicationException
    {
        public NotFoundException(string name, object key)
            : base(null, $"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
