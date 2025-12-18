using API.Infrastructure.RequestDTOs.Shared;

namespace BonFireAPI.Models.RequestDTOs.User
{
    public class UsersGetRequest : BaseGetRequest
    {
        public UserGetFilterRequest? Filter { get; set; }
    }
}
