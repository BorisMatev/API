using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.Movie
{
    public class MovieGetRequest : BaseGetRequest
    {
        public MovieGetFilterRequest? Filter { get; set; }
    }
}
