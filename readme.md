# Aiursoft CommandFramework

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![NuGet version (Aiursoft.CommandFramework)](https://img.shields.io/nuget/v/Aiursoft.CommandFramework.svg)](https://www.nuget.org/packages/Aiursoft.CommandFramework/)
[![ManHours](https://manhours.aiursoft.cn/r/gitlab.aiursoft.cn/aiursoft/commandframework.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/commits/master?ref_type=heads)

Aiursoft CommandFramework is a framework for building command line tools.

* Auto argument parsing
* Auto help page generation
* Auto version page generation
* Run as service
* Auto dependency injection
* Auto command completion
* Auto logger with `--verbose` option support
* Run as a single command app or a nested command app

With this framework, you can build a modern command line tool with just a few lines of code.

```bash
C:\workspace> ninja.exe

Description:
  Nuget Ninja, a tool for detecting dependencies of .NET projects.

Usage:
  Microsoft.NugetNinja [command] [options]

Options:
  -p, --path <path> (REQUIRED)   Path of the projects to be changed.
  --nuget-server <nuget-server>  If you want to use a customized nuget server instead of the official nuget.org, 
  --token <token>                The PAT token which has privilege to access the nuget server.
  -d, --dry-run                  Preview changes without actually making them
  -v, --verbose                  Show detailed log
  -?, -h, --help                 Show help and usage information

Commands:
  all, all-officials  The command to run all officially supported features.
  remove-deprecated   The command to replace all deprecated packages to new packages.
  upgrade-pkg         The command to upgrade all package references to possible latest and avoid conflicts.
  clean-pkg           The command to clean up possible useless package references.
  clean-prj           The command to clean up possible useless project references.
  ```

## Why this project?

Command-line applications are a great way to automate repetitive tasks or even to be your own productivity tool. But building a command-line application in .NET is not easy. You need to parse the arguments, generate help pages, and so on. This project is designed to help you build a command-line application with just a few lines of code.

## How to install

Run the following command to install `Aiursoft.CommandFramework` to your project from [nuget.org](https://www.nuget.org/packages/Aiursoft.CommandFramework/):

```bash
dotnet add package Aiursoft.CommandFramework
```

## Learn step 1: How to build an executable command handler?

In `Aiursoft.CommandFramework`, a command handler is a class that can be executed as a command.

To build an executable command, you can do:

```csharp
using System.CommandLine;
using System.CommandLine.Invocation;
using Aiursoft.CommandFramework;
using Aiursoft.CommandFramework.Framework;

public class DownloadHandler : ExecutableCommandHandlerBuilder
{
    public static readonly Option<string> Url =
        new(
            aliases: new[] { "-u", "--url" },
            description: "The target url to download.")
    {
        IsRequired = true
    };

    protected override string Name => "download";

    protected override string Description => "Download an HTTP Url.";
    
    protected override Option[] GetCommandOptions() => new Option[]
    {
        // Configure your options here.
        DownloadHandler.Url
    };

    protected override Task Execute(InvocationContext context)
    {
        // Your code entry:
        var url = context.ParseResult.GetValueForOption(DownloadHandler.Url);

        Console.WriteLine($"Downloading file from: {url}...");

        return Task.CompletedTask;
    }
}

// Now you can start your app! Finish your `Program.cs` entry code!
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        return await new SingleCommandApp<DownloadHandler>()
            .WithDefaultOption(DownloadHandler.Url)
            .RunAsync(args);
    }
}
```

Build and run you app!

```bash
$ your-downloader.exe --url https://www.aiursoft.cn
# outputs:
Downloading file from: https://www.aiursoft.cn...
```

## Learn step 2: How to test your command handler?

It it super simple to test a command handler.

Assuming you are using `Microsoft.VisualStudio.TestTools.UnitTesting`, you can do:

```csharp
using Aiursoft.CommandFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class IntegrationTests
{
    private readonly SingleCommandApp<DownloadHandler> _program = new SingleCommandApp<DownloadHandler>()
            .WithDefaultOption(DownloadHandler.Url);

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.TestRunAsync(new[] { "--help" });
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.TestRunAsync(new[] { "--version" });
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.TestRunAsync(new[] { "--wtf" });
        Assert.AreEqual(1, result.ProgramReturn);
    }
}
```

Now write you UT, you can follow this practice:

* Prepare some environment
* Run your command handler
* Assert the result
* Clean up the environment

## Learn step 3: Understand single command app and nested command app

In the previous steps, we have learned how to build a single command app. A single command app is a command line tool that only has one command.

This is useful for something with limited function, like:

* Ping tool
* File download tool
* Web server tool

But in more scenarios, we usually need a command line tool with multiple commands. For example, `git` has many commands like `git clone`, `git commit`, `git push`, etc.

A nested command app is a command line tool that has multiple commands. like:

* [Nuget Ninja](https://gitlab.aiursoft.cn/aiursoft/nugetninja)

## Learn step 4: How to build a nested command app?

To do that, first you need to build several executable command handlers. And then wrap them with a `NavigationCommandHandlerBuilder`:

```csharp
public class NetworkHandler : NavigationCommandHandlerBuilder
{
    protected override string Name => "network";

    protected override string Description => "Network related commands.";

    protected override CommandHandlerBuilder[] GetSubCommandHandlers()
    {
        return
        [
            // Where the `DownloadHandler` is an executable command handler.
            new DownloadHandler()
        ];
    }
}
```

And it's very similar to build a nested command app:

```csharp
// Program.cs of the nested command app.
return await new NestedCommandApp()
    .WithGlobalOptions(CommonOptionsProvider.DryRunOption)
    .WithGlobalOptions(CommonOptionsProvider.VerboseOption)
    .WithFeature(new ConfigHandler())
    .RunAsync(args);
```

Now you can try:

```bash
your-app network download --url https://www.aiursoft.cn
```

## Learn step 5: How to build a command app with dependency injection?

It's easy to build a command app with dependency injection.

Of course, you need to register your services in your `Startup` class first:

```csharp
using Aiursoft.CommandFramework.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Http;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient(nameof(Downloader)).ConfigurePrimaryHttpMessageHandler(() => 
        {
            return new HttpClientHandler()
            {
                AllowAutoRedirect = true,
            };
        });
        services.AddTransient<Downloader>();
    }
}
```

Then in your `Execute(InvocationContext context)` function:

```csharp
// This code is inside an `ExecutableCommandHandlerBuilder`.

protected override async Task Execute(InvocationContext context)
{
  var verbose = context.ParseResult.GetValueForOption(CommonOptionsProvider.VerboseOption);
  var host = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose) // Your own startup class.
            .Build();

  var downloader = host.Services.GetRequiredService<Downloader>(); // Get a service from dependency injection
  await downloader.DownloadWithWatchAsync(url, savePath, blockSize, threads, showProgressBar: !verbose);
}
```

That's it!

## Learn step 6: How to build and configure a background service?

You can even start a background service in your command line tool!

To build a background service, you need to implement an `IHostedService`. Here use `ServerMonitor` as an example:

```csharp

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ServerInitializer>();
        services.AddSingleton<IHostedService, ServerMonitor>();
    }
}

// In your `ExecutableCommandHandlerBuilder`:
protected override async Task Execute(InvocationContext context)
{
    var verbose = context.ParseResult.GetValueForOption(CommonOptionsProvider.VerboseOption);
    var profile = context.ParseResult.GetValueForOption(_profile);
    
    var host = ServiceBuilder
        .CreateCommandHostBuilder<Startup>(verbose)
        .ConfigureServices((hostBuilderContext, services)=>
        {
            // You can configure your services here.
            services.Configure<ProfileConfig>(config =>
            {
                config.Profile = profile;
            });
        })
        .Build();

    await host.StartAsync(); // Now your background service is running!
    await host.WaitForShutdownAsync();
}
```

To read more about how to write an `IHostedService`, please read [this doc](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services) from Microsoft.

## Learn step 7: How to display a progress bar?

It's simple. You can use the `ProgressBar` class from `Aiursoft.CommandFramework`:

```csharp
using Aiursoft.CommandFramework.Models;

ProgressBar? bar = null;
if (showProgress) bar = new ProgressBar();
var completedTasks = 0;
var totalTasks = tasks.Length;

foreach (var task in tasks)
{
    if (showProgress)
    {
        Interlocked.Increment(ref completedTasks);
        // ReSharper disable once AccessToDisposedClosure
        bar?.Report((double)completedTasks / totalTasks);
    }
}
bar?.Dispose();
```

That's it! When running, you may see:

```bash
[########################-----------------------]  50%
```

## Learn step 8: Read more examples

If you want to explore a real project built with this framework, please download the following project:

Single command app:

* [Parser](https://gitlab.aiursoft.cn/anduin/parser) as an example.
* [DotDownload](https://gitlab.aiursoft.cn/aiursoft/dotdownload) as an example.
* [Httping](https://gitlab.aiursoft.cn/aiursoft/httping) as an example.
* [Static](https://gitlab.aiursoft.cn/aiursoft/static) as an example.
* [Dotlang](https://gitlab.aiursoft.cn/aiursoft/dotlang) as an example.

Nested command app:

* [NiBot](https://gitlab.aiursoft.cn/aiursoft/nibot) as an example.
* [Nuget Ninja](https://gitlab.aiursoft.cn/aiursoft/nugetninja) as an example.
* [Happy Recorder](https://gitlab.aiursoft.cn/anduin/HappyRecorder) as an example.

Background service:

* [IPMI Controller](https://gitlab.aiursoft.cn/aiursoft/ipmicontroller) as an example.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
