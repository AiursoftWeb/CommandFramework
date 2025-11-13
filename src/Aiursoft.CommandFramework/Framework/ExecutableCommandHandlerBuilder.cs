using System.CommandLine;

namespace Aiursoft.CommandFramework.Framework;

/// <summary>
/// Command handler builder with options and execute method.
///
/// This can be used to build a command which can be executed.
/// </summary>
public abstract class ExecutableCommandHandlerBuilder : CommandHandlerBuilder
{
    protected abstract Task Execute(ParseResult parseResult);

    protected virtual IEnumerable<Option> GetCommandOptions() => Array.Empty<Option>();

    public override Command BuildAsCommand()
    {
        var command = base.BuildAsCommand();

        foreach (var option in GetCommandOptions())
        {
            command.Options.Add(option);
        }

        command.SetAction(Execute);
        return command;
    }
}
