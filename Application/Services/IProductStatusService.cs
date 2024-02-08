namespace Application.Services
{
    public interface IProductStatusService
    {
        Task<Dictionary<byte, string>> GetProductStatusAsync();
    }
}
