using Microsoft.Extensions.Caching.Memory;
using TennisCleanArchi.Application.Common.Caching;

namespace TennisCleanArchi.Infrastructure.Caching;

public class CachingService : ICachingService
{
    private readonly IMemoryCache _cache;

    public CachingService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T GetOrAdd<T>(string key, Func<T> factory, TimeSpan? absoluteExpiration = null)
    {
        if (_cache.TryGetValue(key, out T value))
            return value;
        value = factory();
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpiration.HasValue)
            options.SetAbsoluteExpiration(absoluteExpiration.Value);
        _cache.Set(key, value, options);
        return value;
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null)
    {
        if (_cache.TryGetValue(key, out T value))
            return value;
        value = await factory();
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpiration.HasValue)
            options.SetAbsoluteExpiration(absoluteExpiration.Value);
        _cache.Set(key, value, options);
        return value;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
