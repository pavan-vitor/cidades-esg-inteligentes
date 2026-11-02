using Json.Schema;
using System.Text.Json.Nodes;
using Xunit;

namespace CidadesESG.Tests.Infrastructure;

public static class JsonSchemaAssertions
{
    public static void AssertMatches(string json, string schemaFile)
    {
        var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", schemaFile);
        var schemaText = File.ReadAllText(schemaPath);
        var schema = JsonSchema.FromText(schemaText);
        var instance = JsonNode.Parse(json);
        Assert.NotNull(instance);

        var result = schema.Evaluate(instance, new EvaluationOptions
        {
            OutputFormat = OutputFormat.List
        });

        Assert.True(result.IsValid, $"JSON nao respeita o schema {schemaFile}: {result}");
    }
}
