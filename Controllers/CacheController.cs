using Applcation.Cache;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp.Controllers;

public record CacheRequest(string Key, string RandomProperty);

public record CacheResponse(string Key, string RandomProperty);

[ApiController]
[Route("api/[controller]")]
public class CacheController(ICacheService cacheService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> StoreRandomProperty([FromBody] CacheRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Key))
        {
            return BadRequest("Key is required.");
        }

        var item = await cacheService.UpsertAsync(request.Key, request.RandomProperty, cancellationToken);

        return Ok(new CacheResponse(item.Key, item.RandomProperty));
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetRandomProperty(string key, CancellationToken cancellationToken)
    {
        var item = await cacheService.GetByKeyAsync(key, cancellationToken);

        if (item is null)
        {
            return NotFound($"No data found for key '{key}'.");
        }

        return Ok(new CacheResponse(item.Key, item.RandomProperty));
    }
}
