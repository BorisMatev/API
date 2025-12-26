namespace BonFireAPI.Models.RequestDTOs.User
{
    public class UserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? Profile_Photo { get; set; }
        public string? Role { get; set; }

    }
}
