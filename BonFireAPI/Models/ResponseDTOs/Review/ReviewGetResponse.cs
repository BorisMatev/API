using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Review;

namespace BonFireAPI.Models.ResponseDTOs.Review
{
    public class ReviewGetResponse : BaseGetResponse<Common.Entities.Review>
    {
        public ReviewGetFilterRequest Filter { get; set; }
    }
}
