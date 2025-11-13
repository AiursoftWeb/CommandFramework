using System.CommandLine;
using System.Reflection;
using Aiursoft.CommandFramework.Abstracts;

namespace Aiursoft.CommandFramework;

public class NestedCommandApp() : CommandApp(new RootCommand(
    (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly())
    .GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ??
    "Unknown usage. Please write the project description in the '.csproj' file."))
{
    public NestedCommandApp WithGlobalOptions(Option option)
    {
        option.Recursive = true;
        RootCommand.Options.Add(option);
        return this;
    }

    public NestedCommandApp WithFeature(Func<ICommandHandlerBuilder> builder)
    {
        RootCommand.Subcommands.Add(builder().BuildAsCommand());
        return this;
    }

    public NestedCommandApp WithFeature(ICommandHandlerBuilder builder)
    {
        RootCommand.Subcommands.Add(builder.BuildAsCommand());
        return this;
    }
}
