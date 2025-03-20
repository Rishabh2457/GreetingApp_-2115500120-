using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

public class RedisCacheService
{
    private readonly IDatabase _cacheDb;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _cacheDb = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await _cacheDb.StringSetAsync(key, jsonData, expiration);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var jsonData = await _cacheDb.StringGetAsync(key);
        return jsonData.HasValue ? JsonSerializer.Deserialize<T>(jsonData) : default;
    }

    public async Task RemoveAsync(string key)
    {
        await _cacheDb.KeyDeleteAsync(key);
    }
}
