using System.CommandLine;
using System.CommandLine.Invocation;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.NiBot.Tests.PingTests;

public class PingHandler : ExecutableCommandHandlerBuilder
{
    protected override string Name => "httping";

    protected override string Description => "Ping a server with HTTP protocol.";

    protected override IEnumerable<Option> GetCommandOptions() => new Option[]
    {
        OptionsProvider.ServerOption,
    };
    
    protected override Task Execute(InvocationContext context)
    {
        var server = context.ParseResult.GetValueForOption(OptionsProvider.ServerOption)!;
        Console.WriteLine($"Pinging {server}...");
        
        return Task.CompletedTask;
    }
}