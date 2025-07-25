using Microsoft.EntityFrameworkCore;
using TTT.Api.Configuration;
using TTT.Data;
using TTT.Data.Development;
using TTT.Data.Repositories;
using TTT.Data.Repositories.Interfaces;
using TTT.Services.Interfaces;
using TTT.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<GameSettings>(options =>
{
    options.BoardSize = int.Parse(Environment.GetEnvironmentVariable("BOARD_SIZE") ?? "3");
    options.WinCondition = int.Parse(Environment.GetEnvironmentVariable("WIN_CONDITION") ?? "3");
});

builder.Services.Configure<DatabaseSettings>(options =>
{
    options.PostgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
    options.PostgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "secret";
    options.PostgresDatabase = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "ttt_db";
    options.PostgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
});

var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "secret";
var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "ttt_db";
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={db}";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TTT API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// For integration tests
public partial class Program { }