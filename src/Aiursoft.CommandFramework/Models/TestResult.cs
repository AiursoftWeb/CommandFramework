using System.CommandLine.IO;

namespace Aiursoft.CommandFramework.Models;

public class TestResult(int programReturn, TestConsole console)
{
    public int ProgramReturn { get; init; } = programReturn;
    public string Output { get; init; } = console.Out.ToString() ?? string.Empty;
    public string Error { get; init; } = console.Error.ToString() ?? string.Empty;
    public TestConsole Console { get; init; } = console;
}