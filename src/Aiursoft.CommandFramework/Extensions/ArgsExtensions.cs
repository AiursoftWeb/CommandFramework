using System.CommandLine;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework.Extensions;

public static class ArgsExtensions
{
    [Obsolete]
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
    
    public static string[] WithDefaultTo(this string[] args, ExecutableCommandHandlerBuilder? builder)
    {
        if (builder is null)
        {
            return args;
        }
        
        if (args.Length == 0 || args.First().StartsWith("-"))
        {
            return new List<string> { builder.Name }.Concat(args).ToArray();
        }
        
        return args;
    }
}