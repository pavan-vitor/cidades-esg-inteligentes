 using CidadesESG.Tests.Infrastructure;
using System.Net;
using Xunit;

namespace CidadesESG.Tests;

public class HealthTests
{
    [Fact]
    public async Task Health_Deve_Retornar_200()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Indicadores_Deve_Retornar_200()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/indicadores");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
