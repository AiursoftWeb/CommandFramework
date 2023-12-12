using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.CommandFramework.Tests.PingTests;

[TestClass]
public class PingTests
{
    private readonly SingleCommandApp _program = new SingleCommandApp(new PingHandler())
        .WithDefaultOption(OptionsProvider.ServerOption);
    
    [TestMethod]
    public async Task InvokeNothing()
    {
        var result = await _program.TestRunAsync(Array.Empty<string>());
        Assert.AreEqual(1, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokePing()
    {
        var result = await _program.TestRunAsync(new[] { "www.baidu.com" });
        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokePingWithArgs()
    {
        var result = await _program.TestRunAsync(new[] { "--server", "www.baidu.com" });
        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokePingVersion()
    {
        var result = await _program.TestRunAsync(new[] { "--version" });
        Assert.AreEqual(0, result.ProgramReturn);
    }
}