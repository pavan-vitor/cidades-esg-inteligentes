using CidadesESG.Tests.Infrastructure;
using Reqnroll;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace CidadesESG.Tests.StepDefinitions;

[Binding]
public sealed class IndicadoresSteps : IDisposable
{
    private readonly TestWebApplicationFactory _factory = new();
    private readonly HttpClient _client;
    private HttpResponseMessage? _response;
    private object? _payload;
    private int _id;
    private string _body = string.Empty;

    public IndicadoresSteps() => _client = _factory.CreateClient();

    [Given("que possuo dados validos de um indicador ESG")]
    public void DadoDadosValidos() => _payload = new { cidade = "Sao Paulo", categoria = "Energia", valor = 75.4m, unidade = "%" };

    [Given("que possuo dados invalidos de um indicador ESG")]
    public void DadoDadosInvalidos() => _payload = new { cidade = "", categoria = "", valor = -10, unidade = "" };

    [Given("que existe um indicador ESG cadastrado")]
    public async Task DadoIndicadorCadastrado()
    {
        var response = await _client.PostAsJsonAsync("/api/indicadores", new { cidade = "Santos", categoria = "Reciclagem", valor = 63.2m, unidade = "%" });
        response.EnsureSuccessStatusCode();
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        _id = doc.RootElement.GetProperty("id").GetInt32();
    }

    [Given("que nao existe um indicador com identificador {int}")]
    public void DadoIndicadorInexistente(int id) => _id = id;

    [When("envio uma requisicao POST para cadastrar o indicador")]
    public async Task QuandoCadastro()
    {
        _response = await _client.PostAsJsonAsync("/api/indicadores", _payload);
        _body = await _response.Content.ReadAsStringAsync();
    }

    [When("consulto esse indicador pelo identificador")]
    public async Task QuandoConsultoExistente() => await Consultar(_id);

    [When("consulto o indicador de identificador {int}")]
    public async Task QuandoConsultoId(int id) => await Consultar(id);

    [When("atualizo esse indicador com dados validos")]
    public async Task QuandoAtualizo()
    {
        _response = await _client.PutAsJsonAsync($"/api/indicadores/{_id}", new { cidade = "Santos", categoria = "Energia limpa", valor = 88.1m, unidade = "%" });
        _body = await _response.Content.ReadAsStringAsync();
    }

    [When("removo esse indicador")]
    public async Task QuandoRemovo()
    {
        _response = await _client.DeleteAsync($"/api/indicadores/{_id}");
        _body = await _response.Content.ReadAsStringAsync();
    }

    [Then("a API deve retornar o status {int}")]
    public void EntaoStatus(int status) => Assert.Equal(status, (int)_response!.StatusCode);

    [Then("o corpo deve conter o indicador cadastrado")]
    public void EntaoCorpoCadastro()
    {
        using var doc = JsonDocument.Parse(_body);
        Assert.True(doc.RootElement.GetProperty("id").GetInt32() > 0);
        Assert.Equal("Sao Paulo", doc.RootElement.GetProperty("cidade").GetString());
    }

    [Then("o corpo deve respeitar o JSON Schema de indicador")]
    public void EntaoSchema() => JsonSchemaAssertions.AssertMatches(_body, "indicador.schema.json");

    [Then("uma nova consulta ao indicador deve retornar {int}")]
    public async Task EntaoNovaConsulta(int status)
    {
        var response = await _client.GetAsync($"/api/indicadores/{_id}");
        Assert.Equal(status, (int)response.StatusCode);
    }

    private async Task Consultar(int id)
    {
        _response = await _client.GetAsync($"/api/indicadores/{id}");
        _body = await _response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _response?.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }
}
