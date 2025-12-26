using BonFireAPI.Models.RequestDTOs.User;
using BonFireAPI.Models.ResponseDTOs;
using BonFireAPI.Models.ResponseDTOs.Movies;
using BonFireAPI.Models.ResponseDTOs.User;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController<User, UserService, UserRequest, UserResponse,UsersGetRequest,UserGetResponse>
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
        }

        protected override Expression<Func<User, bool>> GetFilter(UsersGetRequest model)
        {
            model.Filter ??= new UserGetFilterRequest();

            return
                u =>
                    (string.IsNullOrEmpty(model.Filter.Username) || u.Username.Contains(model.Filter.Username)) &&
                    (string.IsNullOrEmpty(model.Filter.Email) || u.Email.Contains(model.Filter.Email)) &&
                    (string.IsNullOrEmpty(model.Filter.Role) || u.Role.Contains(model.Filter.Role));
        }

        protected override void PopulateGetResponse(UsersGetRequest request, UserGetResponse response)
        {
            response.Filter = request.Filter;
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

        [HttpPut]
        [Route("update-profile")]
        public IActionResult UpdateProfile([FromForm] UserUpdateRequest request)
        {
            var loggedUserId = int.Parse(User.FindFirst("loggedUserId")?.Value);
            var entity = service.GetById(loggedUserId);

            if (entity == null)
                return BadRequest("Not found");

            if (request.Username != null) entity.Username = request.Username;
            if (request.Password != null)
            {
                if (request.OldPassword == entity.Password)
                {
                    entity.Password = request.Password;
                }
                else
                {
                    return BadRequest("Incorect password " + request.OldPassword + " " + entity.Password); 
                }
            }
            if (request.Email != null) entity.Email = request.Email;
            if (request.Role != null) entity.Role = request.Role;
            if (request.Profile_Photo != null) entity.Profile_Photo = photoService.GetPhotoName(request.Profile_Photo);

            service.Save(entity);

            return Ok();
        }

        [HttpGet]
        [Route("get-favorite")]
        public IActionResult GetFavorite()
        {
            MovieService movieService = new MovieService();

            var loggedUserId = int.Parse(User.FindFirst("loggedUserId")?.Value);

            var user = service.GetById(loggedUserId);

            if (user == null)
            {
                return BadRequest("Not found");
            }

            var movies = user.FavoriteMovies.Select(x => new MovieResponse
            {
                Id = x.Movie.Id,
                Title = x.Movie.Title,
                Release_Date = x.Movie.Release_Date,
                Rating = x.Movie.Rating,
                CoverPath = x.Movie.CoverPath,
                Reviews = x.Movie.Reviews.Select(x => new ReviewResponse
                {
                    Comment = x.Comment,
                    Id = x.Id,
                    UserName = x.User.Username,
                    Rating = x.Rating,
                    UserId = x.UserId
                }).ToList(),
                DirectorName = x.Movie.Director.Name,
                Genres = x.Movie.Genres.Select(x => x.Genre.Name).ToList(),
                Actors = x.Movie.Actors.Select(x => x.Actor.Name).ToList()
            }); ;

            if (movies == null)
            {
                return BadRequest();
            }

            return Ok(movies);
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
