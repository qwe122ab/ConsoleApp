using ConsoleApp.Data;
using ConsoleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Controllers;

public record CacheRequest(string Key, string RandomProperty);

public record CacheResponse(string Key, string RandomProperty);

[ApiController]
[Route("api/[controller]")]
public class CacheController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> StoreRandomProperty([FromBody] CacheRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Key))
        {
            return BadRequest("Key is required.");
        }

        var existingItem = await dbContext.CacheItems
            .SingleOrDefaultAsync(item => item.Key == request.Key, cancellationToken);

        if (existingItem is null)
        {
            existingItem = new CacheItem
            {
                Key = request.Key,
                RandomProperty = request.RandomProperty
            };
            dbContext.CacheItems.Add(existingItem);
        }
        else
        {
            existingItem.RandomProperty = request.RandomProperty;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new CacheResponse(existingItem.Key, existingItem.RandomProperty));
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetRandomProperty(string key, CancellationToken cancellationToken)
    {
        var item = await dbContext.CacheItems
            .AsNoTracking()
            .SingleOrDefaultAsync(cacheItem => cacheItem.Key == key, cancellationToken);

        if (item is null)
        {
            return NotFound($"No data found for key '{key}'.");
        }

        return Ok(new CacheResponse(item.Key, item.RandomProperty));
    }
}
