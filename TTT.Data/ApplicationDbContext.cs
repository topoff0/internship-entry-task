using Microsoft.EntityFrameworkCore;
using TTT.Core.Entities.GameEntities;
using TTT.Core.Entities.UserEntities;

namespace TTT.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Players)
                .WithMany(p => p.Games);
        }
    }
}