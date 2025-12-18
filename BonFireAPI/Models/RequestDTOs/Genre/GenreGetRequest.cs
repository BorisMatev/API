using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.Genre
{
    public class GenreGetRequest : BaseGetRequest
    {
        public GenreGetFilterRequest? Filter { get; set; }
    }
}
