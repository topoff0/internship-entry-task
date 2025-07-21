namespace TTT.Api.DTO.GameDtos
{
    public class MoveRequest
    {
        public Guid PlayerId { get; set; }
        public string PlayerSign { get; set; } = default!;
        public PositionDto Position { get; set; } = default!;
    }
}