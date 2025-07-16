namespace TTT.Core.Entities
{
    public readonly struct Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int ToIndex(int boardSize) => Y * boardSize + X;

        public static Coordinate FromIndex(int index, int boardSize)
        {
            int y = index / boardSize;
            int x = index % boardSize;
            return new Coordinate(x, y);
        }

        public override string ToString() => $"({X}, {Y})";
    }
}