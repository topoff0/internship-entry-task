namespace TTT.Api.DTO.Game
{
    public class CreateGameRequest
    {
        public Guid PlayerXId { get; set; }
        public Guid PlayerOId { get; set; }        
    }
}