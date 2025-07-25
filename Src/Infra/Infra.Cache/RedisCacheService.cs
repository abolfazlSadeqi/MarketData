using Core.Interface;
using StackExchange.Redis;
using System.Text.Json;

namespace Infra.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
        _redis = redis;

    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var val = await _db.StringGetAsync(key);
        return val.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(val!);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// اضافه کردن دیتا به کش
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public async Task AppendToListAsync<T>(string key, T value, int? maxLength = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.ListRightPushAsync(key, json);

        if (maxLength.HasValue)
            await _db.ListTrimAsync(key, -maxLength.Value, -1);
    }

    public async Task<List<T>> GetListAsync<T>(string key, int? count = null)
    {
        var length = count ?? 100;
        var values = await _db.ListRangeAsync(key, -length, -1);

        return values.Select(v => JsonSerializer.Deserialize<T>(v!)).Where(x => x != null).ToList()!;
    }

    /// <summary>
    /// استخراج لیست نمادها از تاریخچه قیمت نمادها
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllSymbolNamesFromHistoryAsync()
    {
        var server = _redis.GetServer(_redis.GetEndPoints().First());

        var symbolNames = new List<string>();

        foreach (var key in server.Keys(pattern: "symbol:*:history"))
        {
            var keyStr = key.ToString(); 
            var parts = keyStr.Split(':');

            if (parts.Length >= 3)
            {
                var symbolName = parts[1]; 
                symbolNames.Add(symbolName);
            }
        }


        return symbolNames;
    }


}

