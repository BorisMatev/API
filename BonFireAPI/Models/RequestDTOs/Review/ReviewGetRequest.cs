using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.Review
{
    public class ReviewGetRequest : BaseGetRequest
    {
        public ReviewGetFilterRequest? Filter { get; set; }
    }
}
