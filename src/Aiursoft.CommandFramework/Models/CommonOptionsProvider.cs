using System.CommandLine;

namespace Aiursoft.CommandFramework.Models;

public static class CommonOptionsProvider
{
    public static readonly Option<string> PathOptions = new(
        aliases: ["--path", "-p"],
        description: "Path of the videos to be parsed.")
    {
        IsRequired = true
    };

    public static readonly Option<bool> DryRunOption = new(
        aliases: ["--dry-run", "-d"],
        description: "Preview changes without actually making them");

    public static readonly Option<bool> VerboseOption = new(
        aliases: ["--verbose", "-v"],
        description: "Show detailed log");
}
