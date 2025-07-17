using Microsoft.EntityFrameworkCore;
using TTT.Core.Entities.GameEntities;
using TTT.Data.Repositories.Interfaces;

namespace TTT.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GameRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Game game)
        {
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Game> GetAsync(Guid gameId)
        {
            var game = await _dbContext.Games
                                       .Include(g => g.Players)
                                       .FirstOrDefaultAsync(g => g.Id == gameId);
            return game ?? throw new KeyNotFoundException($"Game with ID {gameId} not found");
        }

        public async Task UpdateAsync(Game game)
        {
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
        }
    }
}