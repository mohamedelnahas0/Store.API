namespace Store.Service.Services.CacheService
{
    public interface ICachService
    {
        Task SetCachResponseAsync(string key, object response, TimeSpan timeToLive);
        Task<string> GetCachResponseAsync(string key);
    }
}
