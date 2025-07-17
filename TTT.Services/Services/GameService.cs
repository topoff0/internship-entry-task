using TTT.Core.Entities.GameEntities;
using TTT.Core.Enums;
using TTT.Services.Interfaces;

namespace TTT.Services.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<Guid, Game> _games = [];

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

            game.Status = CheckGameStatus(game);

            return game;
        }

        public Game GetGame(Guid gameId)
        {
            if (!_games.TryGetValue(gameId, out var game))
                throw new KeyNotFoundException("Game not found");

            return game;
        }


        private static GameStatus CheckGameStatus(Game game)
        {
            var board = game.Board;
            int size = game.BoardSize;
            int winCondition = game.WinCondition;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var current = board[size * y + x];
                    if (current == Sign.Empty) continue;

                    if (HasLine(game, x, y, 1, 0, current) ||   // →
                        HasLine(game, x, y, 0, 1, current) ||   // ↓
                        HasLine(game, x, y, 1, 1, current) ||   // ↘
                        HasLine(game, x, y, 1, -1, current))    // ↗
                    {
                        return current == Sign.X ? GameStatus.XWon : GameStatus.OWon;
                    }
                }
            }

            return board.All(s => s != Sign.Empty) ? GameStatus.Draw : GameStatus.InProgress;
        }

        private static bool HasLine(Game game, int x, int y, int dx, int dy, Sign player)
        {
            var board = game.Board;
            int size = game.BoardSize;
            int win = game.WinCondition;

            for (int i = 0; i < win; i++)
            {
                int nx = x + dx * i;
                int ny = y + dy * i;

                if (nx < 0 || ny < 0 || nx >= size || ny >= size)
                    return false;

                var index = ny * size + nx;
                if (board[index] != player)
                    return false;
            }

            return true;
        }
    }
}