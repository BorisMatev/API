using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Genre;

namespace BonFireAPI.Models.ResponseDTOs.Genre
{
    public class GenreGetResponse : BaseGetResponse<GenreResponse>
    {
        public GenreGetFilterRequest Filter { get; set; }
    }
}
