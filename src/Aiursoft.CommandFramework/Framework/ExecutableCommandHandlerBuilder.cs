using System.CommandLine;
using System.CommandLine.Invocation;

namespace Aiursoft.CommandFramework.Framework;

/// <summary>
/// Command handler builder with options and execute method.
///
/// This can be used to build a command which can be executed.
/// </summary>
public abstract class ExecutableCommandHandlerBuilder : CommandHandlerBuilder
{
    protected abstract Task Execute(InvocationContext context);

    protected virtual IEnumerable<Option> GetCommandOptions() => Array.Empty<Option>();

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
}