using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace ConsoleApp.Controllers;

public record CacheRequest(string Key, string RandomProperty);

public record CacheResponse(string Key, string RandomProperty);

[ApiController]
[Route("api/[controller]")]
public class CacheController(IDistributedCache distributedCache) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> StoreRandomProperty([FromBody] CacheRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Key))
        {
            return BadRequest("Key is required.");
        }

        var response = new CacheResponse(request.Key, request.RandomProperty);
        var serializedValue = JsonSerializer.Serialize(response);

        await distributedCache.SetStringAsync(request.Key, serializedValue, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetRandomProperty(string key, CancellationToken cancellationToken)
    {
        var serializedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(serializedValue))
        {
            return NotFound($"No data found for key '{key}'.");
        }

        var cachedValue = JsonSerializer.Deserialize<CacheResponse>(serializedValue);

        return Ok(cachedValue);
    }
}
