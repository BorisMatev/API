namespace BonFireAPI.Models.ResponseDTOs.User
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Profile_Photo { get; set; }
        public string Role { get; set; }

        public virtual List<string> Reviews { get; set; }
        public virtual List<string> FavoriteMovies { get; set; }
    }
}
