using System.CommandLine;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework.Framework;

public abstract class NavigationCommandHandlerBuilder : ICommandHandlerBuilder
{
    public abstract string Name { get; }
    public abstract string Description { get; }

    public virtual string[] Alias => Array.Empty<string>();

    public virtual CommandHandlerBuilder[] GetSubCommandHandlers() => Array.Empty<CommandHandlerBuilder>();

    public virtual Option[] GetCommandOptions() => Array.Empty<Option>();

    public virtual Command BuildAsCommand()
    {
        var command = new Command(Name, Description);
        foreach (var alias in Alias)
        {
            command.AddAlias(alias);
        }

        foreach (var option in GetCommandOptions())
        {
            command.AddOption(option);
        }

        foreach (var subcommand in GetSubCommandHandlers())
        {
            command.AddCommand(subcommand.BuildAsCommand());
        }
        return command;
    }
}