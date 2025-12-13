namespace BonFireAPI.Models.ResponseDTOs.Actor
{
    public class ActorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public List<string> Movies { get; set; }
    }
}
