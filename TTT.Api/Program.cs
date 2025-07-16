using TTT.Api.Configuration;

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

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
