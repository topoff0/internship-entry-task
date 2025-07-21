using TTT.Core.Entities.GameEntities;

namespace TTT.Services.Interfaces
{
    public interface IGameService
    {
        public Task<Game> CreateGameAsync(int boardSize, int winCondition, Guid playerXId, Guid playerOId);
        public Task<CachedMoveResult> MakeMoveAsync(Move move);
        public Task<Game> GetGameAsync(Guid gameId);
    }
}