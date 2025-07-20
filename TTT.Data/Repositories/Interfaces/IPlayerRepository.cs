using TTT.Core.Entities.UserEntities;

namespace TTT.Data.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        public Task<User> GetUserAsync(Guid userId);
        public Task<Player> GetPlayerWithGamesAsync(Guid playerId);
        public Task CreateAsync(Player player);
        public Task UpdateAsync(Player player);
        public Task SaveChangesAsync();
    }
}