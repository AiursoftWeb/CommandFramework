// 删掉：using System.CommandLine.IO;

namespace Aiursoft.CommandFramework.Models;

public class TestResult(int programReturn, string stdOut, string stdErr)
{
    public int ProgramReturn { get; } = programReturn;
    public string StdOut { get; } = stdOut;
    public string StdErr { get; } = stdErr;
}
