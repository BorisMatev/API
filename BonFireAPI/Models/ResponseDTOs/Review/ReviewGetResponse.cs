using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.Review;

namespace BonFireAPI.Models.ResponseDTOs.Review
{
    public class ReviewGetResponse : BaseGetResponse<ReviewResponse>
    {
        public ReviewGetFilterRequest Filter { get; set; }
    }
}
