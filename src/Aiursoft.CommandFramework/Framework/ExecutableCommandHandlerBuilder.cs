using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework.Framework;

/// <summary>
/// Command handler builder with options and execute method.
///
/// This can be used to build a command which can be executed.
/// </summary>
public abstract class ExecutableCommandHandlerBuilder : CommandHandlerBuilder
{
    protected abstract Task Execute(InvocationContext context);

    public virtual Option[] GetCommandOptions() => Array.Empty<Option>();

    public override Command BuildAsCommand()
    {
        var command = base.BuildAsCommand();
        
        foreach (var option in GetCommandOptions())
        {
            command.AddOption(option);
        }
        
        command.SetHandler(Execute);
        return command;
    }

    [Obsolete]
    public Task<int> RunAsync(string[] args, IConsole? console = null, Option? defaultOption = null)
    {
        var thisCommand = BuildAsCommand();
        var program= new CommandLineBuilder(thisCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
        return program.InvokeAsync(args.WithDefaultTo(defaultOption), console);
    }

    [Obsolete]
    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var thisCommand = BuildAsCommand();
        var testConsole = new TestConsole();
        var program = new CommandLineBuilder(thisCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
        var programReturn = await program.InvokeAsync(args.WithDefaultTo(defaultOption), testConsole);
        return new TestResult(programReturn, testConsole);
    }
}