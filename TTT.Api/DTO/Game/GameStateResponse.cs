using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTT.Api.DTO.Game
{
    public class GameStateResponse
    {
        public Guid GameId { get; set; }
        public string Status { get; set; } = default!;
        public string CurrentPlayerSign { get; set; } = default!;
        public string[] Board { get; set; } = default!;
        public int MoveCount { get; set; }
    }
}