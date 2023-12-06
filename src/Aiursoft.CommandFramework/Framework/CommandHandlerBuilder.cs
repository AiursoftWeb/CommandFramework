using System.CommandLine;
using System.CommandLine.Invocation;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework.Framework;

public abstract class CommandHandlerBuilder : NavigationCommandHandlerBuilder
{
    protected abstract Task Execute(InvocationContext context);

    public override Command BuildAsCommand()
    {
        var command = base.BuildAsCommand();
        command.SetHandler(Execute);
        return command;
    }
}