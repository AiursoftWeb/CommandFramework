using System.CommandLine;

namespace Aiursoft.CommandFramework.Extensions;

public static class ArgsExtensions
{
    public static string[] WithDefaultOption(this string[] args, Option? option)
    {
        if (option is null)
        {
            return args;
        }

        if (args.Length == 0)
        {
            return args;
        }

        if (args.First().StartsWith("-"))
        {
            return args;
        }

        // The first item in args is not an option, so we add the default option to the args.
        return new List<string> { $"{option.Name}" }.Concat(args).ToArray();
    }
}
