namespace Applcation.Cache;

public interface ICacheService
{
    Task<CacheServiceResult?> GetByKeyAsync(string key, CancellationToken cancellationToken);
    Task<CacheServiceResult> UpsertAsync(string key, string randomProperty, CancellationToken cancellationToken);
}
