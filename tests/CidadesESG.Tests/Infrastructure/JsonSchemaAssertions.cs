 using Json.Schema;
using System.Text.Json;
using Xunit;

namespace CidadesESG.Tests.Infrastructure;

public static class JsonSchemaAssertions
{
    public static void AssertMatches(string json, string schemaFile)
    {
        var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", schemaFile);
        var schemaText = File.ReadAllText(schemaPath);
        var schema = JsonSchema.FromText(schemaText);

        using var document = JsonDocument.Parse(json);
        var instance = document.RootElement;

        var result = schema.Evaluate(instance, new EvaluationOptions
        {
            OutputFormat = OutputFormat.List
        });

        Assert.True(result.IsValid, $"JSON nao respeita o schema {schemaFile}: {result}");
    }
}
