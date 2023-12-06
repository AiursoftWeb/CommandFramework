using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework;

public class AiursoftCommandApp
{
    private readonly Command _rootCommand;

    public AiursoftCommandApp(Command? command = null)
    {
        var descriptionAttribute = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly())
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        _rootCommand = command ?? new RootCommand(descriptionAttribute ?? "Unknown usage. Please write the project description in the '.csproj' file.");
    }

    public AiursoftCommandApp Configure(Action<Command> configure)
    {
        configure(_rootCommand);
        return this;
    }

    public Task<int> RunAsync(string[] args, IConsole? console = null, Option? defaultOption = null)
    {
        var program= new CommandLineBuilder(_rootCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
        return program.InvokeAsync(args.WithDefaultTo(defaultOption), console);
    }
    
    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var testConsole = new TestConsole();
        var program = new CommandLineBuilder(_rootCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
        var programReturn = await program.InvokeAsync(args.WithDefaultTo(defaultOption), testConsole);
        return new TestResult(programReturn, testConsole);
    }
}