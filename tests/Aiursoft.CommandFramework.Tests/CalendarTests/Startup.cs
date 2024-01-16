using Aiursoft.CommandFramework.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.CommandFramework.Tests.CalendarTests;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CalendarRenderer>();
    }
}
