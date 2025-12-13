namespace BonFireAPI.Models.ResponseDTOs.Genre
{
    public class GenreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Movies { get; set; }
    }
}
