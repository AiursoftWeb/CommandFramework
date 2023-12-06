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
}
