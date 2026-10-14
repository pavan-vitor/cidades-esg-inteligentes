using CidadesESG.Api.Data;
using CidadesESG.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CidadesESG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndicadoresController(EsgDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndicadorEsg>>> Get()
        => Ok(await db.Indicadores.OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IndicadorEsg>> GetById(int id)
    {
        var item = await db.Indicadores.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<IndicadorEsg>> Post(IndicadorEsg indicador)
    {
        indicador.Id = 0;
        indicador.DataMedicao = DateTime.UtcNow;
        db.Indicadores.Add(indicador);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = indicador.Id }, indicador);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, IndicadorEsg indicador)
    {
        var atual = await db.Indicadores.FindAsync(id);
        if (atual is null) return NotFound();

        atual.Cidade = indicador.Cidade;
        atual.Categoria = indicador.Categoria;
        atual.Valor = indicador.Valor;
        atual.Unidade = indicador.Unidade;
        atual.DataMedicao = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Indicadores.FindAsync(id);
        if (item is null) return NotFound();

        db.Indicadores.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
