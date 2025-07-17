using TTT.Core.Entities.GameEntities;

namespace TTT.Core.Entities.UserEntities
{
    public class Player : User
    {
        public ICollection<Game> Games { get; set; } = [];
    }
}