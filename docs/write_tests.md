# Write Unit Tests

It's simple to write tests to your CLI app:

If you want to write tests for your command line tool, you can use `TestRunAsync` method to invoke your command:

```csharp
using Aiursoft.CommandFramework;
using Aiursoft.CommandFramework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class IntegrationTests
{
    private readonly AiursoftCommand _program;

    public IntegrationTests()
    {
        _program = new AiursoftCommand()
            .Configure(command =>
            {
                command
                    .AddGlobalOptions()
                    .AddPlugins(
                        // Your plugins
                    );
            });
    }

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.TestRunAsync(new[] { "--help" });

        Assert.AreEqual(0, result.ProgramReturn);
        Assert.IsTrue(result.Output.Contains("Options:"));
        Assert.IsTrue(string.IsNullOrWhiteSpace(result.Error));
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.TestRunAsync(new[] { "--version" });
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.TestRunAsync(new[] { "--wtf" });
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.TestRunAsync(Array.Empty<string>());
        Assert.AreEqual(1, result.ProgramReturn);
    }
}
```

## Single command app

It's also simple to write test for a single command app:

```csharp
using Aiursoft.CommandFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.Httping.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly AiursoftCommandApp _program;

    public IntegrationTests()
    {
        var command = new PingHandler().BuildAsCommand();
        _program = new AiursoftCommandApp(command);
    }

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.TestRunAsync(new[] { "--help" }, defaultOption: OptionsProvider.ServerOption);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.TestRunAsync(new[] { "--version" }, defaultOption: OptionsProvider.ServerOption);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.TestRunAsync(new[] { "--wtf" }, defaultOption: OptionsProvider.ServerOption);
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.TestRunAsync(Array.Empty<string>(), defaultOption: OptionsProvider.ServerOption);
        Assert.AreEqual(1, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokePing()
    {
        // Run
        var result = await _program.TestRunAsync(new[]
        {
            "www.bing.com",
            "-n",
            "1"
        }, defaultOption: OptionsProvider.ServerOption);

        // Assert
        Assert.AreEqual(0, result.ProgramReturn);
    }
    
    [TestMethod]
    public async Task InvokePingFailed()
    {
        // Run
        var result = await _program.TestRunAsync(new[]
        {
            "wrong-value",
            "-n",
            "1",
        }, defaultOption: OptionsProvider.ServerOption);

        // Assert
        Assert.AreEqual(0, result.ProgramReturn);
    }
}
```
