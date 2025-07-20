namespace TTT.Api.DTO.Game
{
    public class GameStateResponse
    {
        public Guid GameId { get; set; }
        public string Status { get; set; } = default!;
        public string CurrentPlayerSign { get; set; } = default!;
        public string[] Board { get; set; } = default!;
        public int MoveNumber { get; set; }
    }
}