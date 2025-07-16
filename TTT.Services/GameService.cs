using TTT.Core.Entities;
using TTT.Core.Enums;

namespace TTT.Services
{
    public class GameService
    {
        private readonly Dictionary<Guid, Game> _games = new();

        public Game CreateGame(int boardSize, int winCondition)
        {
            var game = new Game(boardSize, winCondition);
            _games[game.Id] = game;
            return game;
        }

        public Game MakeMove(Move move)
        {
            if (!_games.TryGetValue(move.GameId, out var game))
                throw new KeyNotFoundException("Game not found");

            var index = move.Position.ToIndex(game.BoardSize);

            if (game.Board[index] != Sign.Empty)
                throw new InvalidOperationException("Cell is occupied");
            if (game.CurrentPlayer != move.Player)
                throw new InvalidOperationException("Not this player's turn");

            game.SetCell(move.Position, move.Player);

            return game;
        }

        public Game GetGame(Guid gameId)
        {
            if (!_games.TryGetValue(gameId, out var game))
                throw new KeyNotFoundException("Game not found");

            return game;
        }
    }
}