using System.CommandLine;

namespace Aiursoft.CommandFramework.Tests.PingTests;

public static class OptionsProvider
{
    public static readonly Option<string> ServerOption = new(
        aliases: new[] { "--server" },
        description: "The server address to ping. Can be a domain name or an IP address.")
    {
        IsRequired = true,
    };
}