using System.CommandLine;
using System.CommandLine.IO;
using Aiursoft.CommandFramework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anduin.HappyRecorder.PluginFramework.Services.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly RootCommand _program;

    public IntegrationTests()
    {
        _program = new RootCommand("Test env.")
            .AddGlobalOptions()
            .AddPlugins();
    }

    [TestMethod]
    public async Task InvokeHelp()
    {
        var console = new TestConsole();
        var result = await _program.InvokeAsync(new[] { "--help" }, console);

        var output = console.Out.ToString();
        var errors = console.Err.ToString();

        Assert.AreEqual(0, result);
        Assert.IsTrue(output.Contains(" "));
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.InvokeAsync(new[] { "--version" });
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.InvokeAsync(new[] { "--wtf" });
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.InvokeAsync(Array.Empty<string>());
        Assert.AreEqual(0, result);
    }
}
