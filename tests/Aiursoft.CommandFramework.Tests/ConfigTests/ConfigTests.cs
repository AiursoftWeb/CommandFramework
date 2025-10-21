namespace Aiursoft.CommandFramework.Tests.ConfigTests;

[TestClass]
public class ConfigTests
{
    private readonly NestedCommandApp _program = new NestedCommandApp()
        .WithFeature(() => new DataHandler())
        .WithFeature(() => new ConfigHandler());
    
    [TestMethod]
    public async Task InvokeData()
    {
        var result = await _program.TestRunAsync(["data"]);

        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeConfig()
    {
        var result = await _program.TestRunAsync(["config", "--help"]);

        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokeConfigGet()
    {
        var result = await _program.TestRunAsync(["config", "get"]);

        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokeConfigAliasGet()
    {
        var result = await _program.TestRunAsync(["cfg", "get"]);

        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokeConfigSet()
    {
        var result = await _program.TestRunAsync(["config", "set"]);

        Assert.AreEqual(0, result.ProgramReturn);
    }
}