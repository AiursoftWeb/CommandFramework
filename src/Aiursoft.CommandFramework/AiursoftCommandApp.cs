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
    private Command _rootCommand;
    private Option? _defaultOptionName;

    private Parser BuildParser()
    {
        return new CommandLineBuilder(_rootCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
    }
    
    private bool IsNestedCommandApp()
    {
        return _rootCommand is RootCommand;
    }
    
    private bool IsSingleCommandApp()
    {
        return !IsNestedCommandApp();
    }
    
    public AiursoftCommandApp()
    {
        var descriptionAttribute = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly())
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        _rootCommand = 
            new RootCommand(descriptionAttribute ?? "Unknown usage. Please write the project description in the '.csproj' file.");
    }

    public AiursoftCommandApp AsSingleCommandApp(ExecutableCommandHandlerBuilder builder)
    {
        _rootCommand = builder.BuildAsCommand();
        return this;
    }

    public AiursoftCommandApp WithGlobalOptions(Option option)
    {
        if (IsSingleCommandApp())
        {
            throw new InvalidOperationException("Single command app should not have global options!");
        }
        _rootCommand.AddGlobalOption(option);
        return this;
    }

    public AiursoftCommandApp WithFeature(Func<ICommandHandlerBuilder> builder)
    {
        if (IsSingleCommandApp())
        {
            throw new InvalidOperationException("Single command app should not have features!");
        }
        _rootCommand.Add(builder().BuildAsCommand());
        return this;
    }

    public AiursoftCommandApp WithFeature(ICommandHandlerBuilder builder)
    {
        if (IsSingleCommandApp())
        {
            throw new InvalidOperationException("Single command app should not have features!");
        }
        _rootCommand.Add(builder.BuildAsCommand());
        return this;
    }
    
    public AiursoftCommandApp WithDefaultOption(Option option)
    {
        if (IsNestedCommandApp())
        {
            throw new InvalidOperationException("Nested command app should not have default option!");
        }
        _defaultOptionName = option;
        return this;
    }

    public Task<int> RunAsync(string[] args)
    {
        return BuildParser().InvokeAsync(
            args
            .WithDefaultOption(_defaultOptionName));
    }
    
    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var testConsole = new TestConsole();
        var programReturn = await BuildParser().InvokeAsync(args
            .WithDefaultOption(_defaultOptionName), testConsole);
        return new TestResult(programReturn, testConsole);
    }
}
