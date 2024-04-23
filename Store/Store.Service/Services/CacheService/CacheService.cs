using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public class CacheService : ICachService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<string> GetCachResponseAsync(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);

            if (cachedResponse.IsNullOrEmpty)
                return null;
            return cachedResponse.ToString();

        }

        public async Task SetCachResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serialzedResponse = JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(key, serialzedResponse, timeToLive);
        }
    }
}
