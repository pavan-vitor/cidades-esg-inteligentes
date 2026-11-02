using CidadesESG.Tests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace CidadesESG.Tests.ApiTests;

public class IndicadoresApiTests
{
    private static object NovoIndicador(string cidade = "Sao Paulo") => new
    {
        cidade,
        categoria = "Energia renovavel",
        valor = 72.5m,
        unidade = "%"
    };

    [Fact]
    public async Task GetIndicadores_DeveRetornar200_ListaJson_EContratoValido()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/api/indicadores");
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
        JsonSchemaAssertions.AssertMatches(json, "indicadores-list.schema.json");
    }

    [Fact]
    public async Task PostIndicador_DeveRetornar201_CorpoJson_EContratoValido()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/indicadores", NovoIndicador());
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("Sao Paulo", doc.RootElement.GetProperty("cidade").GetString());
        Assert.True(doc.RootElement.GetProperty("id").GetInt32() > 0);
        JsonSchemaAssertions.AssertMatches(json, "indicador.schema.json");
    }

    [Fact]
    public async Task PostIndicadorInvalido_DeveRetornar400()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/indicadores", new
        {
            cidade = "",
            categoria = "",
            valor = -1,
            unidade = ""
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("errors", out _));
    }

    [Fact]
    public async Task GetIndicadorPorId_DeveRetornar200_EContratoValido()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var created = await client.PostAsJsonAsync("/api/indicadores", NovoIndicador("Curitiba"));
        var createdJson = JsonDocument.Parse(await created.Content.ReadAsStringAsync());
        var id = createdJson.RootElement.GetProperty("id").GetInt32();

        var response = await client.GetAsync($"/api/indicadores/{id}");
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        JsonSchemaAssertions.AssertMatches(json, "indicador.schema.json");
    }

    [Fact]
    public async Task GetIndicadorInexistente_DeveRetornar404()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/api/indicadores/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PutIndicador_DeveRetornar204_EPersistirAlteracao()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var created = await client.PostAsJsonAsync("/api/indicadores", NovoIndicador());
        var createdDoc = JsonDocument.Parse(await created.Content.ReadAsStringAsync());
        var id = createdDoc.RootElement.GetProperty("id").GetInt32();

        var put = await client.PutAsJsonAsync($"/api/indicadores/{id}", new
        {
            cidade = "Campinas",
            categoria = "Reciclagem",
            valor = 81.2m,
            unidade = "%"
        });
        Assert.Equal(HttpStatusCode.NoContent, put.StatusCode);

        var get = await client.GetAsync($"/api/indicadores/{id}");
        var json = await get.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("Campinas", doc.RootElement.GetProperty("cidade").GetString());
        Assert.Equal("Reciclagem", doc.RootElement.GetProperty("categoria").GetString());
        JsonSchemaAssertions.AssertMatches(json, "indicador.schema.json");
    }

    [Fact]
    public async Task PutIndicadorInexistente_DeveRetornar404()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.PutAsJsonAsync("/api/indicadores/999999", NovoIndicador());
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteIndicador_DeveRetornar204_EDepois404()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var created = await client.PostAsJsonAsync("/api/indicadores", NovoIndicador());
        var createdDoc = JsonDocument.Parse(await created.Content.ReadAsStringAsync());
        var id = createdDoc.RootElement.GetProperty("id").GetInt32();

        var delete = await client.DeleteAsync($"/api/indicadores/{id}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        var get = await client.GetAsync($"/api/indicadores/{id}");
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task DeleteIndicadorInexistente_DeveRetornar404()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var response = await client.DeleteAsync("/api/indicadores/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
