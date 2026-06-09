using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using MiniPay.Application.Interfaces;

namespace MiniPay.Infrastructure.Cache
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {
        private static readonly DistributedCacheEntryOptions DefaultCacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await cache.GetAsync(cacheKey.ToLower(), cancellationToken);

            try
            {
                return cachedResponse == null ? default : JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(cachedResponse));
            }
            catch (JsonException)
            {
                await cache.RemoveAsync(cacheKey, cancellationToken);
                return default;
            }
        }

        public async Task SetAsync<T>(string cacheKey, T data, CancellationToken cancellationToken)
        {
            var serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

            await cache.SetAsync(cacheKey.ToLower(), serializedData, DefaultCacheOptions, cancellationToken);
        }

        public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken)
        {
            return cache.RemoveAsync(cacheKey.ToLower(), cancellationToken);
        }
    }
}
