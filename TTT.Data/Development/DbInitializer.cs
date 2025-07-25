using Microsoft.EntityFrameworkCore;
using TTT.Core.Entities.UserEntities;
using System.Text.Json;

namespace TTT.Data.Development
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext db)
        {
            await db.Database.MigrateAsync();

            if (!await db.Players.AnyAsync())
            {

                var basePath = AppContext.BaseDirectory; // this points to the test assembly output folder
                var seedFilePath = Path.Combine(basePath, "TTT.Data", "Development", "Data", "SeedPlayers.json");

                var players = JsonSerializer.Deserialize<List<PlayerSeedModel>>(seedFilePath);
                
                if (players != null)
                {
                    db.Players.AddRange(players.Select(p => new Player
                    {
                        Id = Guid.Parse(p.Id),
                        Name = p.Name,
                        Email = p.Email,
                        PasswordHash = p.PasswordHash,
                        CreatedAt = DateTime.UtcNow
                    }));
                    await db.SaveChangesAsync();
                }

                await db.SaveChangesAsync();
            }

        }

        private class PlayerSeedModel
        {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string PasswordHash { get; set; } = null!;
        }
    }
}