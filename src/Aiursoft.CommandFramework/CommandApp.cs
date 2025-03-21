﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework;

public abstract class CommandApp
{
    protected readonly Command RootCommand;
    protected Option? DefaultOptionName;

    protected CommandApp(Command rootCommand)
    {
        RootCommand = rootCommand;
    }

    private Parser BuildParser()
    {
        return new CommandLineBuilder(RootCommand)
            .EnablePosixBundling()
            .UseDefaults()
            .Build();
    }
    
    [ExcludeFromCodeCoverage]
    public async Task<int> RunAsync(string[] args, bool noSleep = false)
    {
        var result = await BuildParser().InvokeAsync(
            args
                .WithDefaultOption(DefaultOptionName));
        
        // Sleep await because the console logger might not finish writing logs.
        if (!noSleep)
        {
            await Task.Delay(100);
        }
        
        return result;
    }
    
    public async Task<TestResult> TestRunAsync(string[] args, Option? defaultOption = null)
    {
        var testConsole = new TestConsole();
        var programReturn = await BuildParser().InvokeAsync(args
            .WithDefaultOption(DefaultOptionName), testConsole);
        return new TestResult(programReturn, testConsole);
    }
}