using TTT.Core.Entities.GameEntities;
using TTT.Core.Enums;
using TTT.Data.Repositories.Interfaces;
using TTT.Services.Interfaces;

namespace TTT.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private static readonly Random _random = new();
        private const int SPECIAL_TURN_CHANCE = 10;

        public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }
        public async Task<Game> CreateGameAsync(int boardSize, int winCondition, Guid playerXId, Guid playerOId)
        {
            var game = new Game(boardSize, winCondition, playerXId, playerOId);
            await _gameRepository.CreateAsync(game);

            await LinkGameToPlayersAsync(game, playerXId, playerOId);

            return game;
        }

        public async Task<Game> MakeMoveAsync(Move move)
        {
            var game = await _gameRepository.GetGameAsync(move.GameId);

            if (game.Status != GameStatus.InProgress)
                throw new InvalidOperationException("Game has already ended.");

            if (game.CurrentPlayerSign != move.PlayerSign)
                throw new InvalidOperationException("Not this player's turn");

            if (move.PlayerSign == Sign.X && move.PlayerId != game.PlayerXId
                || move.PlayerSign == Sign.O && move.PlayerId != game.PlayerOId)
                throw new UnauthorizedAccessException("Player is not assigned to this sign in the game");


            var index = move.Position.ToIndex(game.BoardSize);
            if (game.Board[index] != Sign.Empty)
                throw new InvalidOperationException("Cell is occupied");

            bool isSpecialTurn = game.MoveNumber % 3 == 0 && _random.Next(0, 100) < SPECIAL_TURN_CHANCE;
            Sign sign = CalculateSpecialSign(move.PlayerSign, isSpecialTurn);

            game.SetCell(move.Position, sign);
            game.Status = CheckGameStatus(game);
            game.CurrentPlayerSign = NextPlayerSign(game);
            game.MoveNumber++;
            await _gameRepository.UpdateAsync(game);

            return game;
        }

        public async Task<Game> GetGameAsync(Guid gameId)
        {
            var game = await _gameRepository.GetGameAsync(gameId);
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

        private async Task LinkGameToPlayersAsync(Game game, Guid playerXId, Guid playerOId)
        {
            var playerX = await _playerRepository.GetPlayerWithGamesAsync(playerXId);
            var playerO = await _playerRepository.GetPlayerWithGamesAsync(playerOId);

            playerX.Games.Add(game);
            playerO.Games.Add(game);

            await _playerRepository.SaveChangesAsync();
        }

        private static Sign CalculateSpecialSign(Sign currentPlayerSign, bool isSpecialTurn)
        {
            if (currentPlayerSign == Sign.X)
            {
                return isSpecialTurn ? Sign.O : Sign.X;
            }
            // Current player: Sign.O
            return isSpecialTurn ? Sign.X : Sign.O;
        }

        private static Sign NextPlayerSign(Game game)
        {
            if (game.Status == GameStatus.InProgress || game.Status == GameStatus.Draw)
                return Sign.Empty;

            return game.CurrentPlayerSign == Sign.X ? Sign.O : Sign.X;
        }
    }
}