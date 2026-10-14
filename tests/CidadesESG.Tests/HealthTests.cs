using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CidadesESG.Tests;

public class HealthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_Deve_Retornar_200()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Indicadores_Deve_Retornar_200()
    {
        var response = await _client.GetAsync("/api/indicadores");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

using Xunit;
