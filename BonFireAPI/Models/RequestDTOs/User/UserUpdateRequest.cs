namespace BonFireAPI.Models.RequestDTOs.User
{
    public class UserUpdateRequest : UserRequest
    {
        public string? OldPassword { get; set; }
    }
}
