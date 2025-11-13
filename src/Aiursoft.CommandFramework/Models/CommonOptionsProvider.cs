using System.CommandLine;

namespace Aiursoft.CommandFramework.Models;

public static class CommonOptionsProvider
{
    public static readonly Option<string> PathOptions = new Option<string>(
        name: "--path",
        aliases: ["-p"])
    {
        Description = "Path of the videos to be parsed.",
        Required = true
    };

    public static readonly Option<bool> DryRunOption = new(
        name: "--dry-run",
        aliases: ["-d"])
    {
        Description = "Preview changes without actually making them"
    };

    public static readonly Option<bool> VerboseOption = new(
        name: "--verbose",
        aliases: ["-v"])
    {
        Description = "Show detailed log"
    };
}