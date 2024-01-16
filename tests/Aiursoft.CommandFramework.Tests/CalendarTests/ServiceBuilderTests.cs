using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.CommandFramework.Tests.CalendarTests;

[TestClass]
public class ServiceBuilderTests
{
    [TestMethod]
    public void TestServiceBuilder()
    {
        var hostBuilder = ServiceBuilder.CreateCommandHostBuilder<TestStartUp>(true);
        var host = hostBuilder.Build();
        Assert.IsNotNull(hostBuilder);
        Assert.IsNotNull(host);
        Assert.IsNotNull(host.Services);
    }
}

public class TestStartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
    }
}