using BonFireAPI.Models.RequestDTOs.User;
using BonFireAPI.Models.ResponseDTOs;
using BonFireAPI.Models.ResponseDTOs.User;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController<User, UserService, UserRequest, UserResponse>
    {
        PhotoService photoService = new PhotoService();
        UserService service = new UserService();
        protected override void PopulateEntity(User forUpdate, UserRequest model, out string error)
        {
            error = null;
            forUpdate.Email = model.Email;
            forUpdate.Username = model.Username;
            forUpdate.Password = model.Password;
            forUpdate.Profile_Photo = photoService.GetPhotoName(model.Profile_Photo);
            forUpdate.Role = "User";
        }
        protected override void PopulateResponseEntity(UserResponse forUpdate, User model, out string error)
        {
            error = null;
            forUpdate.Id = model.Id;
            forUpdate.Username = model.Username;
            forUpdate.Email = model.Email;
            forUpdate.Role = model.Role;
            forUpdate.Profile_Photo = model.Profile_Photo;
            forUpdate.FavoriteMovies = model.FavoriteMovies.Select(x => x.Movie.Title).ToList();
            forUpdate.Reviews = model.Reviews.Select(x => x.Movie.Title).ToList();
        }
        protected override void OnDelete(User entity, out string error)
        {
            error = null;

            var loggedUserId = User.FindFirst("loggedUserId")?.Value;
            var loggedUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (loggedUserRole == "User" && loggedUserId != entity.Id.ToString())
            {
                error = "Forbiden";
                return;
            }

            photoService.DeletePhoto(entity.Profile_Photo);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public IActionResult Create([FromForm] UserRequest request)
        {
            return base.Create(request);
        }


        [HttpGet]
        [Route("add-to-favorite/{movieId}")]
        public IActionResult AddToFavorite([FromRoute] int movieId) 
        {
            MovieService movieService = new MovieService();

            var loggedUserId = int.Parse(User.FindFirst("loggedUserId")?.Value);

            var user = service.GetById(loggedUserId);
            var movie = movieService.GetById(movieId);

            if (user == null || movie == null)
            {
                return BadRequest("Not found");
            }

            if (user.FavoriteMovies.Any(m => m.MovieId == movie.Id))
            {
                return BadRequest("Movie is already in favorite movies!");
            }

            user.FavoriteMovies.Add(new Favorite 
            {
                MovieId = movie.Id,
                UserId = user.Id,
            });

            service.Save(user);

            return Ok();
        }

        [HttpGet]
        [Route("remove-from-favorite/{movieId}")]
        public IActionResult RemoveFromFavorite([FromRoute] int movieId)
        {
            MovieService movieService = new MovieService();

            var loggedUserId = int.Parse(User.FindFirst("loggedUserId")?.Value);

            var user = service.GetById(loggedUserId);
            var movie = movieService.GetById(movieId);

            if (user == null || movie == null)
            {
                return BadRequest("Not found");
            }

            var favorite = user.FavoriteMovies.FirstOrDefault(m => m.MovieId == movie.Id);

            if (favorite == null)
            {
                return BadRequest("Movie is not added in favorite movies!");
            }

            user.FavoriteMovies.Remove(favorite);

            service.Save(user);

            return Ok();
        }
    }
}
