using API.Infrastructure.ResponseDTOs.Shared;
using BonFireAPI.Models.RequestDTOs.User;

namespace BonFireAPI.Models.ResponseDTOs.User
{
    public class UserGetResponse : BaseGetResponse<Common.Entities.User>
    {
        public UserGetFilterRequest Filter { get; set; }
    }
}
