using System.ComponentModel.DataAnnotations;

namespace CidadesESG.Api.Models;

public class IndicadorEsg
{
    public int Id { get; set; }

    [Required, StringLength(120, MinimumLength = 2)]
    public string Cidade { get; set; } = string.Empty;

    [Required, StringLength(80, MinimumLength = 2)]
    public string Categoria { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required, StringLength(40, MinimumLength = 1)]
    public string Unidade { get; set; } = string.Empty;

    public DateTime DataMedicao { get; set; } = DateTime.UtcNow;
}
