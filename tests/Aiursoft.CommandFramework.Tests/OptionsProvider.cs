﻿using System.CommandLine;
using Aiursoft.CommandFramework.Models;

namespace Aiursoft.CommandFramework.Tests;

public static class OptionsProvider
{
    public static Command AddGlobalOptions(this Command command)
    {
        var options = new Option[]
        {
            CommonOptionsProvider.PathOptions,
            CommonOptionsProvider.DryRunOption,
            CommonOptionsProvider.VerboseOption
        };
        foreach (var option in options)
        {
            command.AddGlobalOption(option);
        }
        return command;
    }
}