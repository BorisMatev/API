namespace BonFireAPI.Models.RequestDTOs.Review
{
    public class ReviewRequest
    {
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
