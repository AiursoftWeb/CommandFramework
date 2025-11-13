namespace Aiursoft.CommandFramework.Tests.PingTests;

[TestClass]
public class PingTests
{
    private readonly SingleCommandApp<PingHandler> _program = new SingleCommandApp<PingHandler>()
        .WithDefaultOption(OptionsProvider.ServerOption);

    [TestMethod]
    public async Task InvokeNothing()
    {
        var result = await _program.TestRunAsync([]);
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokePing()
    {
        var result = await _program.TestRunAsync(["www.baidu.com"]);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokePingWithArgs()
    {
        var result = await _program.TestRunAsync(["--server", "www.baidu.com"]);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokePingVersion()
    {
        var result = await _program.TestRunAsync(["--version"]);
        Assert.AreEqual(0, result.ProgramReturn, result.StdErr);
    }
}
