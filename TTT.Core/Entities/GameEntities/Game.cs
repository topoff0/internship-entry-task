using TTT.Core.Enums;

namespace TTT.Core.Entities.GameEntities
{
    public class Game
    {
        public Guid Id { get; set; }
        
        public int BoardSize { get; set; }
        public int WinCondition { get; set; }
        public Sign[] Board { get; set; } = default!;

        public Sign CurrentPlayerSign { get; set; }  
        public GameStatus Status { get; set; }

        public Guid PlayerXId { get; set; }
        public Guid PlayerOId { get; set; }
        public int MoveNumber { get; set; }

        public Game(int boardSize, int winCondition, Guid playerXId, Guid playerOId)
        {
            Id = Guid.NewGuid();
            BoardSize = boardSize;
            WinCondition = winCondition;
            Board = [.. Enumerable.Repeat(Sign.Empty, BoardSize * BoardSize)];
            CurrentPlayerSign = Sign.X;
            Status = GameStatus.InProgress;

            PlayerXId = playerXId;
            PlayerOId = playerOId;

            MoveNumber = 1;
        }

        public Sign GetCell(Coordinate coord)
            => Board[coord.ToIndex(BoardSize)];
        public void SetCell(Coordinate coord, Sign playerSign)
            => Board[coord.ToIndex(BoardSize)] = playerSign;
    }
}