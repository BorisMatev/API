using Common.Entities;

namespace BonFireAPI.Models.ResponseDTOs.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Profile_Photo { get; set; }
        public string Role { get; set; }

        public virtual List<string> Reviews { get; set; }
        public virtual List<Movie> FavoriteMovies { get; set; }
    }
}
