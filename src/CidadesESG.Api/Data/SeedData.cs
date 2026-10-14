using CidadesESG.Api.Models;

namespace CidadesESG.Api.Data;

public static class SeedData
{
    public static void Initialize(EsgDbContext db)
    {
        if (db.Indicadores.Any()) return;

        db.Indicadores.AddRange(
            new IndicadorEsg { Cidade = "São Paulo", Categoria = "Consumo de Energia", Valor = 72.4m, Unidade = "kWh/hab" },
            new IndicadorEsg { Cidade = "São Paulo", Categoria = "Reciclagem", Valor = 38.2m, Unidade = "%" },
            new IndicadorEsg { Cidade = "Curitiba", Categoria = "Áreas Verdes", Valor = 64.5m, Unidade = "m²/hab" }
        );
        db.SaveChanges();
    }
}
