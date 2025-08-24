namespace TennisCleanArchi.Application.Common.Caching;

public interface ICachingService
{
    T GetOrAdd<T>(string key, Func<T> factory, TimeSpan? absoluteExpiration = null);
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null);
    void Remove(string key);
}
