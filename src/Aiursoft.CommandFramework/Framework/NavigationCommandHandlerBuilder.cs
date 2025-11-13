using System.CommandLine;

namespace Aiursoft.CommandFramework.Framework;

/// <summary>
/// Command handler builder with sub command handlers.
/// </summary>
public abstract class NavigationCommandHandlerBuilder : CommandHandlerBuilder
{
    protected virtual CommandHandlerBuilder[] GetSubCommandHandlers() => [];

    public override Command BuildAsCommand()
    {
        var command = base.BuildAsCommand();

        foreach (var subcommand in GetSubCommandHandlers())
        {
            command.Subcommands.Add(subcommand.BuildAsCommand());
        }
        return command;
    }
}