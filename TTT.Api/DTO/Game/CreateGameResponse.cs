namespace TTT.Api.DTO.Game
{
    public class CreateGameResponse
    {
        public Guid GameId { get; set; }
        public int BoardSize { get; set; }
        public int WinCondition { get; set; }
        public Guid PlayerXId { get; set; }
        public Guid PlayerOId { get; set; }
        public string Status { get; set; } = default!;
    }
}