using Common.Entities;

namespace BonFireAPI.Models.ResponseDTOs.Movies
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverPath { get; set; }
        public string Release_Date { get; set; }
        public double Rating { get; set; }

        public string DirectorName { get; set; }

        public List<string> Genres { get; set; }
        public List<string> Actors { get; set; }
        public List<ReviewResponse> Reviews { get; set; }
    }
}
