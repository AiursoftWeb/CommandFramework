using System.CommandLine.IO;

namespace Aiursoft.CommandFramework.Models;

public class TestResult
{
    public TestResult(int programReturn, TestConsole console)
    {
        ProgramReturn = programReturn;
        Output = console.Out.ToString() ?? string.Empty;
        Error = console.Error.ToString() ?? string.Empty;
    }
    
    public int ProgramReturn { get; init; }
    public string Output { get; init; }
    public string Error { get; init; }
}