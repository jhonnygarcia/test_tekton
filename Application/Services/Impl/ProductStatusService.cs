using Application.Components;
using Application.Utility;
using Domain.Main.Enums;

namespace Application.Services.Impl
{
    internal class ProductStatusService : IProductStatusService
    {
        private const int CacheTimeInMinutes = 5;
        private const string KeyProductSatus = "status-product";

        private readonly IDataCacheService _cache;
        public ProductStatusService(IDataCacheService cache)
        {
            _cache = cache;
        }

        public async Task<Dictionary<byte, string>> GetProductStatusAsync()
        {
            var data = await _cache.GetAsync<Dictionary<byte, string>>(KeyProductSatus);
            if (data != null) return data;

            var elements = EnumService.GetEnumItems<ProductStatus>();

            data = elements.ToDictionary(x => (byte)x.Key, x => x.Value);

            await _cache.PutAsync(KeyProductSatus, data, TimeSpan.FromMinutes(CacheTimeInMinutes));
            return data;
        }
    }
}
