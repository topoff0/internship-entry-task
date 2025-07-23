using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTT.Api.DTO.GameDtos;
using TTT.Core.Enums;
using TTT.Tests.Integration.Fixtures;

namespace TTT.Tests.Integration
{
    public class GameControllerTests : IClassFixture<TTTWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GameControllerTests(TTTWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateGame_ReturnsCreateGameResponse()
        {
            // Arrange
            var request = new CreateGameRequest
            {
                PlayerXId = TTTWebApplicationFactory.TetsPlayerXId,
                PlayerOId = TTTWebApplicationFactory.TetsPlayerOId
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/game", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var gameResponse = await response.Content.ReadFromJsonAsync<CreateGameResponse>();
            Assert.NotNull(gameResponse);
            Assert.Equal(request.PlayerXId, gameResponse.PlayerXId);
            Assert.Equal(request.PlayerOId, gameResponse.PlayerOId);
            Assert.True(gameResponse.BoardSize > 0);
            Assert.True(gameResponse.WinCondition > 0);
            Assert.Equal("InProgress", gameResponse.Status);
        }

        [Fact]
        public async Task CreateGame_PlayerNotFound_Returns404()
        {
            // Arrange
            var request = new CreateGameRequest
            {
                PlayerXId = Guid.Empty,
                PlayerOId = Guid.Empty
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/game", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            Assert.Contains("Player not found", problemDetails?.Title);
        }

        [Fact]
        public async Task MakeMove_ValidMove_ReturnsUpdatedGameState()
        {
            // Arrange
            var createRequest = new CreateGameRequest
            {
                PlayerXId = TTTWebApplicationFactory.TetsPlayerXId,
                PlayerOId = TTTWebApplicationFactory.TetsPlayerOId
            };
            var createResponse = await _client.PostAsJsonAsync("api/game", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdGameResponse = await createResponse
                .Content.ReadFromJsonAsync<CreateGameResponse>();
            Assert.NotNull(createdGameResponse);

            var moveRequest = new MoveRequest
            {
                PlayerId = createRequest.PlayerXId,
                PlayerSign = Sign.X.ToString(),
                Position = new PositionDto { Row = 0, Column = 0 }
            };

            // Act
            var moveResponse =
                await _client.PostAsJsonAsync($"api/game/{createdGameResponse.GameId}/moves", moveRequest);

            //Assert
            moveResponse.EnsureSuccessStatusCode();
            var gameState = await moveResponse.Content.ReadFromJsonAsync<GameStateResponse>();
            Assert.NotNull(gameState);
            Assert.Equal(createdGameResponse.GameId, gameState.GameId);
            Assert.NotNull(moveResponse.Headers.ETag);
        }

        [Fact]
        public async Task MakeMove_InvalidMove_Returns400()
        {
            // Arrange
            var createRequest = new CreateGameRequest
            {
                PlayerXId = TTTWebApplicationFactory.TetsPlayerXId,
                PlayerOId = TTTWebApplicationFactory.TetsPlayerOId
            };
            var createResponse = await _client.PostAsJsonAsync("api/game", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdGameResponse = await createResponse
                .Content.ReadFromJsonAsync<CreateGameResponse>();
            Assert.NotNull(createdGameResponse);

            var invalidMoveRequest = new MoveRequest
            {
                PlayerId = createRequest.PlayerXId,
                PlayerSign = Sign.X.ToString(),
                Position = new PositionDto { Row = -1, Column = -1 }
            };

            // Act
            var moveResponse =
                await _client.PostAsJsonAsync($"api/game/{createdGameResponse.GameId}/moves", invalidMoveRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, moveResponse.StatusCode);
            var problemDetails = await moveResponse.Content.ReadFromJsonAsync<ProblemDetails>();
            Assert.Contains("Invalid move", problemDetails?.Title);
        }

        [Fact]
        public async Task GetGame_ExistingGame_ReturnsGameState()
        {
            // Arrange
            var createRequest = new CreateGameRequest
            {
                PlayerXId = TTTWebApplicationFactory.TetsPlayerXId,
                PlayerOId = TTTWebApplicationFactory.TetsPlayerOId
            };
            var createResponse = await _client.PostAsJsonAsync("api/game", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createdGameResponse = await createResponse
                .Content.ReadFromJsonAsync<CreateGameResponse>();
            Assert.NotNull(createdGameResponse);

            // Act
            var getResponse = await _client.GetAsync($"api/game/{createdGameResponse.GameId}");
            getResponse.EnsureSuccessStatusCode();

            // Assert
            var gameState = await getResponse.Content.ReadFromJsonAsync<GameStateResponse>();
            Assert.NotNull(gameState);
            Assert.Equal(createdGameResponse.GameId, gameState.GameId);
            Assert.True(gameState.MoveNumber > 0);
        }

        [Fact]
        public async Task GetGame_NotExistingGame_Returns404()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"api/game/{randomId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            Assert.Contains("Game not found", problemDetails?.Title);
        }
    }
}