using System.Text.Json;
using CloudBank.Application.Interfaces;
using StackExchange.Redis;

namespace CloudBank.Infrastructure.Caching;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var database = _redis.GetDatabase();

        var value = await database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null)
    {
        var database = _redis.GetDatabase();

        var serializedValue = JsonSerializer.Serialize(value);

        if (expiry.HasValue)
        {
            await database.StringSetAsync(
                key,
                serializedValue,
                new Expiration(expiry.Value));
        }
        else
        {
            await database.StringSetAsync(
                key,
                serializedValue);
        }
    }

    public async Task RemoveAsync(string key)
    {
        var database = _redis.GetDatabase();

        await database.KeyDeleteAsync(key);
    }
}