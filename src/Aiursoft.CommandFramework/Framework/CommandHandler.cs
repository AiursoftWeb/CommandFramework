using System.CommandLine;
using System.CommandLine.Invocation;

namespace Aiursoft.CommandFramework.Framework;

public abstract class CommandHandler
{
    public abstract string Name { get; }
    public abstract string Description { get; }

    protected abstract Task Execute(InvocationContext context);

    public virtual string[] Alias => Array.Empty<string>();

    public virtual CommandHandler[] GetSubCommandHandlers() => Array.Empty<CommandHandler>();

    public virtual Option[] GetCommandOptions() => Array.Empty<Option>();

    public Command BuildAsCommand()
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
        
        command.SetHandler(Execute);

        return command;
    }
}
