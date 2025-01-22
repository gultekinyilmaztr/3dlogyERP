using System.Text.Json;
using _3dlogyERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace _3dlogyERP.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IConfiguration _configuration;

        public RedisCacheService(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _configuration = configuration;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!value.HasValue)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            if (expirationTime.HasValue)
                await _db.StringSetAsync(key, serializedValue, expirationTime);
            else
                await _db.StringSetAsync(key, serializedValue);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
    }
}
