using Microsoft.EntityFrameworkCore;
using TTT.Core.Entities.GameEntities;
using TTT.Core.Entities.UserEntities;
using TTT.Data.Repositories.Interfaces;

namespace TTT.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PlayerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Player player)
        {
            await _dbContext.Players.AddAsync(player);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Player> GetPlayerWithGamesAsync(Guid playerId)
        {
            var player = await _dbContext.Players
                                         .Include(p => p.Games)
                                         .FirstOrDefaultAsync(p => p.Id == playerId);

            return player ?? throw new KeyNotFoundException($"Player with ID: {playerId} not found");
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == userId);

            return user ?? throw new KeyNotFoundException($"User with ID: {userId} not found");
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
        }
    }
}