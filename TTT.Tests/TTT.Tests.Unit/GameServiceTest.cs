using Microsoft.Extensions.Caching.Memory;
using Moq;
using TTT.Core.Entities.GameEntities;
using TTT.Core.Enums;
using TTT.Data.Repositories.Interfaces;
using TTT.Services.Services;

namespace TTT.Tests.Unit
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock = new();
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new();
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        private GameService CreateService() => new GameService(
            _gameRepositoryMock.Object,
            _playerRepositoryMock.Object,
            _memoryCache);

        private Game CreateTestGame()
        {
            return new Game(3, 3, Guid.NewGuid(), Guid.NewGuid())
            {
                Status = GameStatus.InProgress,
                MoveNumber = 1,
                CurrentPlayerSign = Sign.X
            };
        }

        [Fact]
        public async Task MakeMoveAsync_FirstCall_MakesMove()
        {
            // Arrange
            var game = CreateTestGame();
            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 0)
            };
            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            // Act
            var result = await service.MakeMoveAsync(move);

            // Assert
            Assert.Equal(game.Id, result.Game.Id);
            Assert.NotNull(result.ETag);
        }

        [Fact]
        public async Task MakeMoveAsync_SecondCall_ReturnsCachedResult()
        {
            // Arrange
            var game = CreateTestGame();
            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 0)
            };
            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();
            var firstResult = await service.MakeMoveAsync(move);

            // Act
            var secondResult = await service.MakeMoveAsync(move);

            // Assert
            Assert.Equal(firstResult.ETag, secondResult.ETag);
            Assert.Same(firstResult.Game, secondResult.Game);
        }

        [Fact]
        public async Task MakeMoveAsync_Idempotency_ReturnsSameResultOnRaceCondition()
        {
            var game = CreateTestGame();
            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(1, 1)
            };

            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            var results = await Task.WhenAll(
                service.MakeMoveAsync(move),
                service.MakeMoveAsync(move)
            );

            var result1 = results[0];
            var result2 = results[1];
            

            Assert.Equal(result1.ETag, result2.ETag);
            Assert.Equal(result1.Game.Id, result2.Game.Id);
        }

        [Fact]
        public async Task MakeMoveAsync_Throws_WhenNotPlayersTurn()
        {
            // Arrange
            var game = CreateTestGame();
            game.CurrentPlayerSign = Sign.O;

            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 0)
            };

            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.MakeMoveAsync(move));
        }

        [Fact]
        public async Task MakeMoveAsync_Throws_WhenCellOccupied()
        {
            // Arrange
            var game = CreateTestGame();
            game.SetCell(new Coordinate(0, 0), Sign.X);

            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 0)
            };

            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.MakeMoveAsync(move));
        }

        [Fact]
        public async Task MakeMoveAsync_Throws_WhenPlayerNotAssigned()
        {
            // Arrange
            var game = CreateTestGame();

            var move = new Move
            {
                GameId = game.Id,
                PlayerId = Guid.NewGuid(), // Not assigned
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 0)
            };

            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.MakeMoveAsync(move));
        }

        [Fact]
        public async Task MakeMoveAsync_Throws_WhenGameEnded()
        {
            // Arrange
            var game = CreateTestGame();
            game.Status = GameStatus.XWon;

            var move = new Move
            {
                GameId = game.Id,
                PlayerId = game.PlayerXId,
                PlayerSign = Sign.X,
                Position = new Coordinate(0, 1)
            };

            _gameRepositoryMock.Setup(r => r.GetGameAsync(game.Id))
                               .ReturnsAsync(game);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.MakeMoveAsync(move));
        }
    }
}

