using TTT.Core.Enums;

namespace TTT.Core.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public int BoardSize { get; set; }
        public int WinCondition { get; set; }
        public Sign[] Board { get; set; } = default!;
        public Sign CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }

        public Game(int boardSize, int winCondition)
        {
            Id = Guid.NewGuid();
            BoardSize = boardSize;
            WinCondition = winCondition;
            Board = [.. Enumerable.Repeat(Sign.Empty, BoardSize * BoardSize)];
            CurrentPlayer = Sign.X;
            Status = GameStatus.InProgress;
        }

        public Sign GetCell(Coordinate coord)
            => Board[coord.ToIndex(BoardSize)];
        public void SetCell(Coordinate coord, Sign playerSign)
            => Board[coord.ToIndex(BoardSize)] = playerSign;
    }
}