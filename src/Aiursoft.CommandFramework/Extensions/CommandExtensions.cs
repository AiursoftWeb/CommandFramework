using System.CommandLine;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework.Extensions;

public static class CommandExtensions
{
    public static Command AddPlugins(this Command command, params IPlugin[] pluginInstallers)
    {
        foreach (var plugin in pluginInstallers)
        {
            foreach (var pluginFeature in plugin.Install())
            {
                command.Add(pluginFeature.BuildAsCommand());
            }
        }

        return command;
    }

    public static string[] WithDefaultTo(this string[] args, Option? option)
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

        return new List<string> { $"--{option.Name}" }.Concat(args).ToArray();
    }
}