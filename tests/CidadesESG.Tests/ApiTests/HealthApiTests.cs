using CidadesESG.Tests.Infrastructure;
using System.Net;
using System.Text.Json;
using Xunit;

namespace CidadesESG.Tests.ApiTests;

public class HealthApiTests
{
    [Fact]
    public async Task GetHealth_DeveRetornar200_JsonValido_EContratoValido()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("healthy", doc.RootElement.GetProperty("status").GetString());
        JsonSchemaAssertions.AssertMatches(json, "health.schema.json");
    }
}
