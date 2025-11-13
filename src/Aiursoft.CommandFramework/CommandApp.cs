using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework;

public abstract class CommandApp(Command rootCommand)
{
    protected readonly Command RootCommand = rootCommand;
    protected Option? DefaultOptionName;

    [ExcludeFromCodeCoverage]
    public async Task<int> RunAsync(string[] args, bool noSleep = false)
    {
        var finalArgs = args.WithDefaultOption(DefaultOptionName);
        var parseResult = RootCommand.Parse(finalArgs);
        var result = await parseResult.InvokeAsync();

        // Sleep await because the console logger might not finish writing logs.
        if (!noSleep)
        {
            await Task.Delay(100);
        }

        return result;
    }

    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var stdOut = new StringWriter();
        var stdErr = new StringWriter();
        var invocationConfig = new InvocationConfiguration
        {
            Output = stdOut,
            Error = stdErr
        };

        var finalArgs = args.WithDefaultOption(DefaultOptionName);
        var parseResult = RootCommand.Parse(finalArgs);
        var programReturn = await parseResult.InvokeAsync(invocationConfig);
        return new TestResult(programReturn, stdOut.ToString(), stdErr.ToString());
    }
}
