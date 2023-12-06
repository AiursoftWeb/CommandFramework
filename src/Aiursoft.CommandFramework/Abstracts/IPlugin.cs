namespace Aiursoft.CommandFramework.Abstracts;

public interface IPlugin
{
    public ICommandHandlerBuilder[] Install();
}
