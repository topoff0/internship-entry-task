using TTT.Core.Entities;

namespace TTT.Services.Interfaces
{
    public interface IGameService
    {
        Game CreateGame(int boardSize, int winCondition);
        Game MakeMove(Move move);
        Game GetGame(Guid gameId);
    }
}