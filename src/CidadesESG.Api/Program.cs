using CidadesESG.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataDir = Environment.GetEnvironmentVariable("DATA_DIR") ?? "data";
Directory.CreateDirectory(dataDir);
var dbPath = Path.Combine(dataDir, "cidades-esg.db");
builder.Services.AddDbContext<EsgDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTimeOffset.UtcNow
}));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EsgDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}

app.Run();

public partial class Program { }
