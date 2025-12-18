using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Movie;
using Common.Entities;

namespace BonFireAPI.Models.ResponseDTOs.Movies
{
    public class MovieGetResponse : BaseGetResponse<Movie>
    {
        public MovieGetFilterRequest Filter { get; set; }
    }
}
