using TTT.Core.Enums;

namespace TTT.Core.Entities
{
    public class Move
    {
        public Guid GameId { get; set; }
        public Coordinate Position { get; set; }
        public Sign Player { get; set; }
    }
}