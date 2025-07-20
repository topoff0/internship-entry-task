using TTT.Core.Entities.GameEntities;

namespace TTT.Data.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<Game> GetGameAsync(Guid gameId);
        public Task CreateAsync(Game game);
        public Task UpdateAsync(Game game); 
    }
}