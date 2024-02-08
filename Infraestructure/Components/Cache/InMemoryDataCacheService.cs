using Application.Components;
using Microsoft.Extensions.Caching.Memory;

namespace Infraestructure.Components.Cache
{
    public class InMemoryDataCacheService : IDataCacheService
    {
        public readonly TimeSpan DefualtCacheTime = TimeSpan.FromMinutes(10);
        private readonly IMemoryCache _cache;
        public InMemoryDataCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync<T>(string key)
        {
            var data = _cache.Get<T>(key);
            return Task.FromResult(data);
        }

        public Task<object> GetAsync(string key)
        {
            var data = _cache.Get(key);            
            return Task.FromResult(data);
        }

        public Task PutAsync(string key, object value, TimeSpan time)
        {
            _cache.Set(key, value, time);
            return Task.CompletedTask;
        }

        public Task PutAsync(string key, object value)
        {
            _cache.Set(key, value, DefualtCacheTime);
            return Task.CompletedTask;
        }
    }
}
