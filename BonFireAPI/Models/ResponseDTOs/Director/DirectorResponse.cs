namespace BonFireAPI.Models.ResponseDTOs.Director
{
    public class DirectorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public List<string> Movies { get; set; }
    }
}
