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

                var seedFilePath = Path.Combine(AppContext.BaseDirectory, "Development", "Data", "SeedPlayers.json");

                if (!File.Exists(seedFilePath))
                    throw new FileNotFoundException("SeedPlayers.json not found", seedFilePath);
    
                var jsonString = await File.ReadAllTextAsync(seedFilePath);

                var players = JsonSerializer.Deserialize<List<PlayerSeedModel>>(jsonString);

                
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