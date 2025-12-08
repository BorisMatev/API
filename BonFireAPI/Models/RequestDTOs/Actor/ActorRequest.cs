namespace BonFireAPI.Models.RequestDTOs.Actor
{
    public class ActorRequest
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
    }
}
