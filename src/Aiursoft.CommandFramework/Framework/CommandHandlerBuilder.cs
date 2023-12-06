using System.CommandLine;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework.Framework;

public abstract class CommandHandlerBuilder : ICommandHandlerBuilder
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    
    public virtual string[] Alias => Array.Empty<string>();

    public virtual Command BuildAsCommand()
    {
        var command = new Command(Name, Description);
        foreach (var alias in Alias)
        {
            command.AddAlias(alias);
        }
        
        return command; 
    }
}