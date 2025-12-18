namespace BonFireAPI.Models.RequestDTOs.Movie
{
    public class MovieGetFilterRequest
    {
        public int? DirectorId { get; set; }
        public string? Title { get; set; }
        public string? Release_Date { get; set; }
        public double? Rating { get; set; }
        public string? Actor { get; set; }
        public string? Genre { get; set; }
    }
}
