using System.CommandLine.Invocation;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework.Tests.ConfigTests;

public class DataHandler : NavigationCommandHandlerBuilder
{
    protected override string Name => "data";
    protected override string Description => "data control.";

    protected override string[] Alias => ["dat"];
}

public class ConfigHandler : NavigationCommandHandlerBuilder
{
    protected override string Name => "config";
    protected override string Description => "config control.";

    protected override string[] Alias => ["cfg"];

    protected override CommandHandlerBuilder[] GetSubCommandHandlers()
    {
        return
        [
            new GetConfig(),
            new SetConfig()
        ];
    }
}

public class GetConfig : ExecutableCommandHandlerBuilder
{
    protected override string Name => "get";
    protected override string Description => "get config.";
    protected override Task Execute(InvocationContext context)
    {
        Console.WriteLine("get config");
        return Task.CompletedTask;
    }
}

public class SetConfig : ExecutableCommandHandlerBuilder
{
    protected override string Name => "set";
    protected override string Description => "set config.";
    protected override Task Execute(InvocationContext context)
    {
        Console.WriteLine("set config");
        return Task.CompletedTask;
    }
}