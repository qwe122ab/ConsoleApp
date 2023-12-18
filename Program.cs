using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                .WriteTo.Sentry(o =>
                {
                    o.Dsn = "https://7a6c711dffe3b3b5e50a5e85fc24ce18@o4506416479535104.ingest.sentry.io/4506416521805824";
                })
                .WriteTo.Console();
            }).Build().Run();
    }
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<RunWorker>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}

public class RunWorker : IHostedService
{
    private readonly Serilog.ILogger _logger;

    public RunWorker(Serilog.ILogger logger, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _lifetime = lifetime;
    }

    private readonly IHostApplicationLifetime _lifetime;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var b = 123;
        var c = 312;

        _logger.Error("lets gooooo");

        _lifetime.StopApplication();
        return Task.CompletedTask;

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
