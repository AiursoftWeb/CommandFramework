using System.CommandLine;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework.Framework;

public abstract class CommandHandlerBuilder : ICommandHandlerBuilder
{
    protected abstract string Name { get; }
    protected abstract string Description { get; }

    protected virtual string[] Alias => [];

    public virtual Command BuildAsCommand()
    {
        var command = new Command(Name, Description);
        foreach (var alias in Alias)
        {
            command.Aliases.Add(alias);
        }
        
        return command; 
    }
}