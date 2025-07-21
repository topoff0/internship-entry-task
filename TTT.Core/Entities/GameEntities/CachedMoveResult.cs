namespace TTT.Core.Entities.GameEntities
{
    public class CachedMoveResult
    {
        public Game Game { get; set; } = default!;
        public string ETag { get; set; } = default!;
    }
}