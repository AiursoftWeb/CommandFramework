# Aiursoft CommandFramework

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![NuGet version (Aiursoft.CommandFramework)](https://img.shields.io/nuget/v/Aiursoft.CommandFramework.svg)](https://www.nuget.org/packages/Aiursoft.CommandFramework/)

Aiursoft CommandFramework is a framework for building command line tools.

* Auto argument parsing
* Auto help page generation
* Auto version page generation

With this framework, you can build a command line tool with just a few lines of code.

## Why this project?

Command-line applications are a great way to automate repetitive tasks or even to be your own productivity tool. But building a command-line application in .NET is not easy. You need to parse the arguments, generate help pages, and so on. This project is designed to help you build a command-line application with just a few lines of code.

## Installation

Run the following command to install `Aiursoft.CommandFramework` to your project from [nuget.org](https://www.nuget.org/packages/Aiursoft.CommandFramework/):

```bash
dotnet add package Aiursoft.CommandFramework
```

![diagram](./demo/diagram.png)

In your `YourProject.Core`, write an options provider:

```csharp
namespace YourProject.Core

public static class OptionsProvider
{
    public static RootCommand AddGlobalOptions(this RootCommand command)
    {
        var options = new Option[]
        {
            CommonOptionsProvider.DryRunOption,
            CommonOptionsProvider.VerboseOption
        };
        foreach (var option in options)
        {
            command.AddGlobalOption(option);
        }
        return command;
    }
}
```

Now you can write your executable:

```csharp
using System.CommandLine;
using System.Reflection;
using Aiursoft.CommandFramework.Extensions;

namespace YourProject.Executable;

public class Program
{
    public static async Task Main(string[] args)
    {
        var descriptionAttribute = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        var program = new RootCommand(descriptionAttribute ?? "Unknown usage.")
            .AddGlobalOptions()
            .AddPlugins(
                new CalendarPlugin()
            );

        await program.InvokeAsync(args);
    }
}
```

Yes, I know you need to write plugins for your executable!

Now try to write a plugin:

```csharp
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;

namespace YourProject.Plugins.Calendar;

public class CalendarPlugin : IPlugin
{
    public CommandHandler[] Install()
    {
        return new CommandHandler[]
        {
            new CalendarHandler(),
        };
    }
}


public class CalendarHandler : CommandHandler
{
    public override string Name => "calendar";

    public override string Description => "Show calendar.";

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(Execute, CommonOptionsProvider.VerboseOption);
    }

    private Task Execute(bool verbose)
    {
        var services = ServiceBuilder
            .BuildServices<Startup>(verbose)
            .BuildServiceProvider();
        
        var calendar = services.GetRequiredService<CalendarRenderer>();
        calendar.Render();
        return Task.CompletedTask;
    }
}

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CalendarRenderer>();
    }
}

public class CalendarRenderer
{
    public void Render()
    {
        Console.WriteLine("Hello world!");
    }
}
```

## Advanced Usage

Of course, handlers can be nested:

```csharp
public class GetHandler : CommandHandler
{
    public override string Name => "get";

    public override string Description => "Get something.";

    public override CommandHandler[] GetSubCommandHandlers()
    {
        return new CommandHandler[]
        {
            new DataHandler(),
            new HistoryHandler(),
            new CalendarHandler()
        };
    }
}
```

When your app starts, it just works!

```bash
$ yourapp get calendar
Hello world!
```

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
