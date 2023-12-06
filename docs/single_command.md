# Single Command app

```bash
dotnet add package Aiursoft.CommandFramework
```

Write your Program.cs:

```csharp
// Program.cs
using Aiursoft.CommandFramework;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.Httping;

var command = new PingHandler().BuildAsCommand();

return await new AiursoftCommandApp(command)
    .RunAsync(args.WithDefaultTo(OptionsProvider.ServerOption));
```

Yes, of course you need to build your own `PingHandler` and `OptionsProvider`!

```csharp
// OptionsProvider.cs
using System.CommandLine;

namespace Aiursoft.Httping;

public static class OptionsProvider
{
    public static readonly Option<string> ServerOption = new(
        aliases: new[] { "--server" },
        description: "The server address to ping. Can be a domain name or an IP address.")
    {
        IsRequired = true,
    };
    
    public static readonly Option<int> CountOption = new(
        aliases: new[] { "--count", "-n" },
        getDefaultValue: () => 4,
        description: "The number of echo requests to send. The default is 4.")
    {
        IsRequired = false,
    };
    
    public static readonly Option<int> TimeoutOption = new(
        aliases: new[] { "--timeout", "-w" },
        getDefaultValue: () => 5000,
        description: "Timeout in milliseconds to wait for each reply. The default is 5000.")
    {
        IsRequired = false,
    };
    
    public static readonly Option<int> IntervalOption = new(
        aliases: new[] { "--interval", "-i" },
        getDefaultValue: () => 1000,
        description: "Time in milliseconds to wait between pings. The default is 1000.")
    {
        IsRequired = false,
    };
    
    public static readonly Option<bool> InsecureOption = new(
        aliases: new[] { "--insecure", "-k" },
        getDefaultValue: () => false,
        description: "Allow insecure server connections when using SSL.")
    {
        IsRequired = false,
    };
    
    public static readonly Option<bool> QuietOption = new(
        aliases: new[] { "--quiet", "-q" },
        getDefaultValue: () => false,
        description: "Quiet output. Nothing is displayed except the summary lines at startup time and when finished.")
    {
        IsRequired = false,
    };
}
```

And the `PingHandler`:

```csharp

public class PingHandler : CommandHandler
{
    public override string Name => "httping";

    public override string Description => "Ping a server with HTTP protocol."; 

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(Execute);
    }
    
    public override Option[] GetCommandOptions() => new Option[]
    {
        OptionsProvider.ServerOption,
        OptionsProvider.CountOption,
        OptionsProvider.TimeoutOption,
        OptionsProvider.IntervalOption,
        OptionsProvider.InsecureOption,
        OptionsProvider.QuietOption,
        CommonOptionsProvider.VerboseOption,
    };

    private static async Task Execute(InvocationContext context)
    {
        var server = context.ParseResult.GetValueForOption(OptionsProvider.ServerOption)!;
        var count = context.ParseResult.GetValueForOption(OptionsProvider.CountOption);
        var timeout = context.ParseResult.GetValueForOption(OptionsProvider.TimeoutOption);
        var interval = context.ParseResult.GetValueForOption(OptionsProvider.IntervalOption);
        var insecure = context.ParseResult.GetValueForOption(OptionsProvider.InsecureOption);
        var quiet = context.ParseResult.GetValueForOption(OptionsProvider.QuietOption);
        var verbose = context.ParseResult.GetValueForOption(CommonOptionsProvider.VerboseOption);

        var host = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose)
            .Build();

        await host.StartAsync();

        var pingWorker = host.Services.GetRequiredService<PingWorker>();
        await pingWorker.HttpPing(
            url: server, 
            count: count, 
            timeout: TimeSpan.FromMilliseconds(timeout),
            interval: TimeSpan.FromMilliseconds(interval), 
            insecure: insecure,
            quiet: quiet);
    }
}

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<PingWorker>();
    }
}

public class PingWorker
{
    public async Task HttpPing(string url, int count, TimeSpan timeout, TimeSpan interval, bool insecure = false, bool quiet = false)
    {
        var handler = new HttpClientHandler();
        if (insecure)
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }
        var client = new HttpClient(handler) { Timeout = timeout };
        var uri = new UriBuilder(url);

        var total = TimeSpan.Zero;
        var minimum = TimeSpan.MaxValue;
        var maximum = TimeSpan.Zero;
        var loss = 0;

        for (var i = 0; i < count; i++)
        {
            try
            {
                var elapsed = await SendHttpRequest(client, uri);
                if (!quiet)
                {
                    Console.WriteLine(
                        $"{i + 1}: {elapsed.response.Version}, {elapsed.response.RequestMessage?.RequestUri?.Host}:{elapsed.response.RequestMessage?.RequestUri?.Port}, code={elapsed.response.StatusCode}, size={elapsed.response.Content.Headers.ContentLength} bytes, time={elapsed.TimeElapsed.TotalMilliseconds} ms");
                }

                // Statistics
                total += elapsed.TimeElapsed;
                minimum = TimeSpan.FromMilliseconds(Math.Min(minimum.TotalMilliseconds,
                    elapsed.TimeElapsed.TotalMilliseconds));
                maximum = TimeSpan.FromMilliseconds(Math.Max(maximum.TotalMilliseconds,
                    elapsed.TimeElapsed.TotalMilliseconds));

                // Wait
                var wait = interval - elapsed.TimeElapsed;
                if (wait > TimeSpan.Zero)
                {
                    await Task.Delay(wait);
                }
            }
            catch (TaskCanceledException)
            {
                loss++;
                Console.WriteLine($"PING {url} timeout");
            }
            catch (Exception ex)
            {
                loss++;
                Console.WriteLine($"PING {url} {ex.Message}");
            }
        }

        var average = total / count;

        Console.WriteLine();
        Console.WriteLine($"Ping statistics for {url}");
        Console.WriteLine(
            $"    Packets: Sent = {count}, Received = {count - loss}, Lost = {loss} ({loss * 100 / count}% loss),");
        Console.WriteLine($"Approximate round trip times in milli-seconds:");
        Console.WriteLine(
            $"    Minimum = {minimum.TotalMilliseconds}ms, Maximum = {maximum.TotalMilliseconds}ms, Average = {average.TotalMilliseconds}ms");
    }

    private async Task<(HttpResponseMessage response, TimeSpan TimeElapsed)> SendHttpRequest(HttpClient client, UriBuilder uri)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync(uri.Uri);
        watch.Stop();
        return (response, watch.Elapsed);
    }
}
```

That's it, now run your app!

```bash
$ httping https://google.com
1: 1.1, www.bing.com:443, code=OK, size=40744 bytes, time=837.8612 ms
2: 1.1, www.bing.com:443, code=OK, size=40675 bytes, time=418.0189 ms
3: 1.1, www.bing.com:443, code=OK, size=40675 bytes, time=413.7146 ms
4: 1.1, www.bing.com:443, code=OK, size=40675 bytes, time=407.5908 ms

Ping statistics for www.bing.com
    Packets: Sent = 4, Received = 4, Lost = 0 (0% loss),
Approximate round trip times in milli-seconds:
    Minimum = 407.5908ms, Maximum = 837.8612ms, Average = 519.2964ms
```
