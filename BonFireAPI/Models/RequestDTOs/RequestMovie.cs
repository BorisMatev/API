namespace BonFireAPI.Models.RequestDTOs
{
    public class RequestMovie
    {
        public string Title { get; set; }
        public string Release_Date { get; set; }
        public double Rating { get; set; }
        public IFormFile Photo { get; set; }
    }
}
