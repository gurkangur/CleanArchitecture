using System;
using System.Threading.Tasks;
using Domain.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistence.Caching.ContractResolvers;

namespace Persistence.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new PrivateSetterContractResolver()
        };

        private static DistributedCacheEntryOptions DefaultCacheOptions = new DistributedCacheEntryOptions();

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> Get<T>(string key)
        {
            
            var data = await _distributedCache.GetStringAsync(key);
            return await DeserializeJson<T>(data);
        }

        public async Task<string> Get(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task<bool> IsSet(string key)
        {
            var data = await Get(key);
            return data != null;
        }

        public async Task Set(string key, string value, TimeSpan? ttl = null)
        {
            await _distributedCache.SetStringAsync(key, value, ttl.HasValue ? new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            } : DefaultCacheOptions);
        }

        public async Task Set<T>(string key, T value, TimeSpan? ttl = null)
        {
            var serialized = await SerializeToJson(value);
            await Set(key, serialized, ttl);
        }

        public async Task Unset(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        private async Task<string> SerializeToJson(object o)
        {
            return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(o, _serializerSettings));
        }

        private async Task<T> DeserializeJson<T>(string json)
        {
            if (json == null)
                return default(T);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(json, typeof(T));
            }

            var obj = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json, _serializerSettings));
            return obj;
        }
    }
}