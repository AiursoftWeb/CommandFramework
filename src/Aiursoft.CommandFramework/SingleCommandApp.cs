using System.CommandLine;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework;

public class SingleCommandApp : CommandApp
{
    public SingleCommandApp(ExecutableCommandHandlerBuilder builder)
        : base (builder.BuildAsCommand())
    {
    }
    
    public SingleCommandApp WithDefaultOption(Option option)
    {
        DefaultOptionName = option;
        return this;
    }
}