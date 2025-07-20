using TTT.Core.Enums;

namespace TTT.Core.Entities.GameEntities
{
    public class Move
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; } = default!;
        public Coordinate Position { get; set; }
        public Sign PlayerSign { get; set; }
    }
}