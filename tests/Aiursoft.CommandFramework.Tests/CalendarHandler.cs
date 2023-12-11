using System.CommandLine.Invocation;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.CommandFramework.Tests;

public class CalendarHandler : ExecutableCommandHandlerBuilder
{
    public override string Name => "calendar";

    public override string Description => "Show calendar.";

    protected override Task Execute(InvocationContext context)
    {
        var verbose = context.ParseResult.GetValueForOption(CommonOptionsProvider.VerboseOption);
        var services = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose)
            .Build()
            .Services;
        
        var calendar = services.GetRequiredService<CalendarRenderer>();
        calendar.Render();
        return Task.CompletedTask;
    }
}