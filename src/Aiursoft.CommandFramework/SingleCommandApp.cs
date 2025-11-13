using System.CommandLine;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework;

public class SingleCommandApp<T> : CommandApp where T : ExecutableCommandHandlerBuilder, new()
{
    /// <summary>
    /// Initializes a new instance of the SingleCommandApp class.
    /// </summary>
    public SingleCommandApp() : base(BuildRootCommand())
    {
    }

    /// <summary>
    /// Builds the RootCommand by creating a new RootCommand and copying
    /// properties from the handler's command.
    /// </summary>
    private static RootCommand BuildRootCommand()
    {
        var handlerBuilder = new T();

        // Build the temporary handler command to get its properties
        var handlerCommand = handlerBuilder.BuildAsCommand();

        // Create the new RootCommand, copying the description from the handler.
        // This new RootCommand *automatically* has --help and --version.
        var rootCommand = new RootCommand(handlerCommand.Description ?? "A single command application.");

        // Manually copy all options from the handler's command to the new RootCommand.
        foreach (var option in handlerCommand.Options)
        {
            rootCommand.Options.Add(option);
        }

        // Manually copy all arguments from the handler's command to the new RootCommand.
        foreach (var argument in handlerCommand.Arguments)
        {
            rootCommand.Arguments.Add(argument);
        }

        // Set the new RootCommand's handler (Action) to be the handler's Execute method.
        rootCommand.Action = handlerCommand.Action;

        return rootCommand;
    }

    public SingleCommandApp<T> WithDefaultOption(Option option)
    {
        DefaultOptionName = option;
        return this;
    }
}
