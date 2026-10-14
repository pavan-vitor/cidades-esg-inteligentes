using CidadesESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CidadesESG.Api.Data;

public class EsgDbContext(DbContextOptions<EsgDbContext> options) : DbContext(options)
{
    public DbSet<IndicadorEsg> Indicadores => Set<IndicadorEsg>();
}
