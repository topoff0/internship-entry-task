namespace TTT.Api.DTO.Game
{
    public class MakeMoveRequest
    {
        public Guid PlayerId { get; set; }
        public string PlayerSign { get; set; } = default!;
        public PositionDto Position { get; set; } = default!;
    }
}