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
        RootCommand.AddGlobalOption(option);
        return this;
    }

    public NestedCommandApp WithFeature(Func<ICommandHandlerBuilder> builder)
    {
        RootCommand.Add(builder().BuildAsCommand());
        return this;
    }

    public NestedCommandApp WithFeature(ICommandHandlerBuilder builder)
    {
        RootCommand.Add(builder.BuildAsCommand());
        return this;
    }
}
