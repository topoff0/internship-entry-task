using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TTT.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "secret";
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "ttt_db";
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

            var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={db}";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
