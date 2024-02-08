namespace Application.Components
{
    public interface IDataCacheService
    {
        Task PutAsync(string key, object value, TimeSpan time);
        Task PutAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
        Task<object> GetAsync(string key);
    }
}
