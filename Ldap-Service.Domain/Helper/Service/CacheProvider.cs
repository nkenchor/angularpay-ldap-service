using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Ldap_Service.Domain;
    
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _cache;

        public CacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }
        
        public async Task<T> GetFromCache<T>(string key) where T : class
        {
            var cachedData = await _cache.GetStringAsync(key);
            return cachedData == null ? null : JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetCache<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var data = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, data , options);
        }

        public async Task ClearCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
