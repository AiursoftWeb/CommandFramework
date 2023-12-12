using System.CommandLine;
using Aiursoft.CommandFramework.Framework;

namespace Aiursoft.CommandFramework;

public class SingleCommandApp<T> : CommandApp where T : ExecutableCommandHandlerBuilder,new ()
{
    public SingleCommandApp()
        : base (new T().BuildAsCommand())
    {
    }
    
    public SingleCommandApp<T> WithDefaultOption(Option option)
    {
        DefaultOptionName = option;
        return this;
    }
}