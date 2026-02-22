using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CacheItem> CacheItems => Set<CacheItem>();
}
