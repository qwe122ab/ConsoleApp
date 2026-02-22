using Applcation.Cache;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";
    options.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName") ?? "ConsoleApp:";
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ConsoleAppDb"));
builder.Services.AddScoped<ICacheService, CacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
