using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TTT.Api.Configuration;
using TTT.Api.DTO.Game;
using TTT.Core.Entities.GameEntities;
using TTT.Core.Enums;
using TTT.Services.Interfaces;

namespace TTT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly GameSettings _settings;

        public GameController(IGameService gameService, IOptions<GameSettings> gameOptions)
        {
            _gameService = gameService;
            _settings = gameOptions.Value;
        }

        [HttpPost]
        public async Task<ActionResult<CreateGameResponse>> CreateGame([FromBody] CreateGameRequest request)
        {
            try
            {
                int boardSize = _settings.BoardSize;
                int winCondition = _settings.WinCondition;

                var game = await _gameService
                    .CreateGameAsync(boardSize, winCondition,
                                     playerXId: request.PlayerXId,
                                     playerOId: request.PlayerOId);
                var response = new CreateGameResponse
                {
                    GameId = game.Id,
                    BoardSize = boardSize,
                    WinCondition = winCondition,
                    PlayerXId = request.PlayerXId,
                    PlayerOId = request.PlayerOId,
                    Status = game.Status.ToString()
                };
                return CreatedAtAction(nameof(GetGame), new { id = game.Id }, response);
            }
            catch (Exception ex)
            {
                // TODO handle all exceptions
                return Problem(title: "Failed to create game", detail: ex.Message, statusCode: 500);
            }
        }

        [HttpPost("{gameId:guid}/moves")]
        public async Task<ActionResult<GameStateResponse>> MakeMove(Guid gameId, [FromBody] MakeMoveRequest request)
        {
            try
            {
                var move = new Move
                {
                    GameId = gameId,
                    PlayerId = request.PlayerId,
                    PlayerSign = Enum.Parse<Sign>(request.PlayerSign),
                    Position = new Coordinate(request.Position.Row, request.Position.Column)
                };

                var game = await _gameService.MakeMoveAsync(move);

                return Ok(MapGameState(game));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Problem(title: "Unauthorized", detail: ex.Message, statusCode: 403);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: "Invalid move", detail: ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(title: "Server error", detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{gameId:guid}/")]
        public async Task<ActionResult<GameStateResponse>> GetGame(Guid gameId)
        {
            try
            {
                var game = await _gameService.GetGameAsync(gameId);
                return Ok(MapGameState(game));
            }// TODO: handle all exceptions
            catch (Exception ex)
            {
                return Problem(title: "Game not found", detail: ex.Message, statusCode: 404);
            }
        }
        
            private static GameStateResponse MapGameState(Game game)
            {
                return new GameStateResponse
                {
                    GameId = game.Id,
                    Status = game.Status.ToString(),
                    CurrentPlayerSign = game.CurrentPlayerSign.ToString(),
                    Board = [.. game.Board.Select(s => s.ToString())],
                    MoveNumber = game.MoveNumber
                };
            }
    }
}