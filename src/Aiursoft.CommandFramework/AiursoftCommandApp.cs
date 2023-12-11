using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework;

public class AiursoftCommandApp
{
    private readonly Command _rootCommand;

    private Parser BuildParser()
    {
        return new CommandLineBuilder(_rootCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
    }
    
    public AiursoftCommandApp()
    {
        var descriptionAttribute = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly())
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        _rootCommand = 
            new RootCommand(descriptionAttribute ?? "Unknown usage. Please write the project description in the '.csproj' file.");
    }

    public AiursoftCommandApp WithGlobalOptions(Option option)
    {
        _rootCommand.AddGlobalOption(option);
        return this;
    }

    public AiursoftCommandApp WithFeature(Func<ICommandHandlerBuilder> builder)
    {
        _rootCommand.Add(builder().BuildAsCommand());
        return this;
    }

    public AiursoftCommandApp WithFeature(ICommandHandlerBuilder builder)
    {
        _rootCommand.Add(builder.BuildAsCommand());
        return this;
    }

    [Obsolete]
    public AiursoftCommandApp Configure(Action<Command> configure)
    {
        configure(_rootCommand);
        return this;
    }
    
    public Task<int> RunAsync(string[] args)
    {
        return BuildParser().InvokeAsync(args);
    }
    
    public Task<int> RunWithDefaultHandler(string[] args, ExecutableCommandHandlerBuilder? defaultHandlerBuilder = null)
    {
        return BuildParser().InvokeAsync(args.WithDefaultHandlerBuilder(defaultHandlerBuilder));
    }
    
    public Task<int> RunWithDefaultOption(string[] args, Option? defaultOption = null)
    {
        return BuildParser().InvokeAsync(args.WithDefaultOption(defaultOption));
    }
    
    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var testConsole = new TestConsole();
        var programReturn = await BuildParser().InvokeAsync(args.WithDefaultOption(defaultOption), testConsole);
        return new TestResult(programReturn, testConsole);
    }
    
    public async Task<TestResult> TestRunWithDefaultHandlerAsync(string[] args, ExecutableCommandHandlerBuilder? defaultHandlerBuilder = null)
    {
        var testConsole = new TestConsole();
        var programReturn = await BuildParser().InvokeAsync(args.WithDefaultHandlerBuilder(defaultHandlerBuilder), testConsole);
        return new TestResult(programReturn, testConsole);
    }
}
