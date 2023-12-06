using System.CommandLine;

namespace Aiursoft.CommandFramework.Abstracts;

public interface ICommandHandlerBuilder
{
    public  Command BuildAsCommand();
}