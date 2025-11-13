using System.CommandLine;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework.Tests.PingTests;

public class PingHandler : ExecutableCommandHandlerBuilder
{
    protected override string Name => "httping";

    protected override string Description => "Ping a server with HTTP protocol.";

    protected override IEnumerable<Option> GetCommandOptions() => new Option[]
    {
        OptionsProvider.ServerOption,
    };

    protected override Task Execute(ParseResult context)
    {
        var server = context.GetValue(OptionsProvider.ServerOption)!;
        Console.WriteLine($"Pinging {server}...");

        return Task.CompletedTask;
    }
}
