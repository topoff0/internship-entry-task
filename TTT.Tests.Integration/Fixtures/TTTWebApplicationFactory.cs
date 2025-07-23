using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using TTT.Core.Entities.UserEntities;
using TTT.Data;

namespace TTT.Tests.Integration.Fixtures
{
    public class TTTWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly PostgreSqlContainer _postgresContainer =
            new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        public string ConnectionString => _postgresContainer.GetConnectionString();

        public static readonly Guid TetsPlayerXId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid TetsPlayerOId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(ConnectionString));
            });
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();

            using var client = CreateClient();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await db.Database.MigrateAsync();

            // Clear tables
            db.Games.RemoveRange(db.Games);
            db.Players.RemoveRange(db.Players);
            await db.SaveChangesAsync();

            db.Players.AddRange(
                new Player
                {
                    Id = TetsPlayerXId,
                    Name = "TextPlayerX",
                    Email = "exampleX@gmail.com",
                    PasswordHash = "passX"
                },
                new Player
                {
                    Id = TetsPlayerOId,
                    Name = "TextPlayerO",
                    Email = "exampleO@gmail.com",
                    PasswordHash = "passO"
                }
            );

            await db.SaveChangesAsync();
        }

        public new Task DisposeAsync() => _postgresContainer.DisposeAsync().AsTask();
    }
}