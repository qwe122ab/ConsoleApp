using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Applcation.Cache;

public class CacheService(AppDbContext dbContext) : ICacheService
{
    public async Task<CacheServiceResult?> GetByKeyAsync(string key, CancellationToken cancellationToken)
    {
        var item = await dbContext.CacheItems
            .AsNoTracking()
            .SingleOrDefaultAsync(cacheItem => cacheItem.Key == key, cancellationToken);

        return item is null ? null : new CacheServiceResult(item.Key, item.RandomProperty);
    }

    public async Task<CacheServiceResult> UpsertAsync(string key, string randomProperty, CancellationToken cancellationToken)
    {
        var existingItem = await dbContext.CacheItems
            .SingleOrDefaultAsync(item => item.Key == key, cancellationToken);

        if (existingItem is null)
        {
            existingItem = new CacheItem
            {
                Key = key,
                RandomProperty = randomProperty
            };
            dbContext.CacheItems.Add(existingItem);
        }
        else
        {
            existingItem.RandomProperty = randomProperty;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CacheServiceResult(existingItem.Key, existingItem.RandomProperty);
    }
}
