namespace Domain.Entities;

public class CacheItem
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public required string RandomProperty { get; set; }
}
