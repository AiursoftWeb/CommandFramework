using Aiursoft.CommandFramework.Models;

[assembly: DoNotParallelize]

namespace Aiursoft.CommandFramework.Tests.CalendarTests;

[TestClass]
public class IntegrationTests
{
    private readonly NestedCommandApp _program = new NestedCommandApp()
        .WithGlobalOptions(CommonOptionsProvider.PathOptions)
        .WithGlobalOptions(CommonOptionsProvider.VerboseOption)
        .WithGlobalOptions(CommonOptionsProvider.DryRunOption)
        .WithFeature(new CalendarHandler());

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.TestRunAsync(["--help"]);

        Assert.AreEqual(0, result.ProgramReturn);
        Assert.Contains("Options:", result.StdOut);
        Assert.IsTrue(string.IsNullOrWhiteSpace(result.StdErr), result.StdErr);
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.TestRunAsync(["--version"]);
        Assert.AreEqual(0, result.ProgramReturn, result.StdErr);
    }

    [TestMethod]
    public async Task InvokeCalendar()
    {
        var result = await _program.TestRunAsync(["calendar", "--path", "something"]);
        Assert.AreEqual(0, result.ProgramReturn, result.StdErr);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.TestRunAsync(["--wtf"]);
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.TestRunAsync([]);
        Assert.AreEqual(1, result.ProgramReturn);
    }
}
