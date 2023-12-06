﻿using System.CommandLine;
using System.CommandLine.Invocation;

namespace Aiursoft.CommandFramework.Framework;

public abstract class CommandHandler : NavigationCommandHandler
{
    protected abstract Task Execute(InvocationContext context);

    public override Command BuildAsCommand()
    {
        var command = base.BuildAsCommand();
        command.SetHandler(Execute);
        return command;
    }
}