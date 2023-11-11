using Aiursoft.CommandFramework.Abstracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aiursoft.CommandFramework.Services;

public static class ServiceBuilder
{
    public static IHostBuilder CreateCommandHostBuilder<T>(bool verbose) where T : IStartUp, new()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureServices(services =>
        {
            var startUp = new T();
            startUp.ConfigureServices(services);
        });

        hostBuilder.ConfigureLogging(logging =>
        {
            logging
                .AddFilter("Microsoft.Extensions", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            logging.AddSimpleConsole(options =>
            {
                options.IncludeScopes = verbose;
                options.SingleLine = true;
                options.TimestampFormat = "mm:ss ";
                options.UseUtcTimestamp = true;
            });

            logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Information);
        });
        
        hostBuilder.UseConsoleLifetime();
        
        return hostBuilder;
    }
}
