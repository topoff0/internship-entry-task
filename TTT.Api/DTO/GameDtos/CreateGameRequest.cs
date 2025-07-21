namespace TTT.Api.DTO.GameDtos
{
    public class CreateGameRequest
    {
        public Guid PlayerXId { get; set; }
        public Guid PlayerOId { get; set; }        
    }
}