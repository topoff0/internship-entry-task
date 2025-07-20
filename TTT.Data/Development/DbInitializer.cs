using Microsoft.EntityFrameworkCore;
using TTT.Core.Entities.UserEntities;

namespace TTT.Data.Development
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext db)
        {
            await db.Database.MigrateAsync();

            if (!await db.Players.AnyAsync())
            {
                db.Players.AddRange(
                    new Player
                    {
                        Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                        Name = "Test User X",
                        Email = "x@example.com",
                        PasswordHash = "test-password-hash-x",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Player
                    {
                        Id = Guid.Parse("6dc8bb68-5a76-4e2b-8a53-dbb7f7d53da0"),
                        Name = "Test User O",
                        Email = "o@example.com",
                        PasswordHash = "test-password-hash-o",
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await db.SaveChangesAsync();
            }

        }
    }
}