namespace CidadesESG.Api.Models;

public class IndicadorEsg
{
    public int Id { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Unidade { get; set; } = string.Empty;
    public DateTime DataMedicao { get; set; } = DateTime.UtcNow;
}
