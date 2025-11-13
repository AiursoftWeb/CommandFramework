using System.CommandLine;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.CommandFramework.Tests.CalendarTests;

public class CalendarHandler : ExecutableCommandHandlerBuilder
{
    protected override string Name => "calendar";

    protected override string Description => "Show calendar.";

    protected override Task Execute(ParseResult context)
    {
        var verbose = context.GetValue(CommonOptionsProvider.VerboseOption);
        var services = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose)
            .Build()
            .Services;

        var calendar = services.GetRequiredService<CalendarRenderer>();
        calendar.Render();
        return Task.CompletedTask;
    }
}
