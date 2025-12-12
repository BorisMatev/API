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
    public class UserController : ControllerBase
    {
        UserService service = new UserService();
        PhotoService photoService = new PhotoService();

        [HttpGet]
        public IActionResult GetAll() 
        {

            var result = service.GetAll()
                .Select(u => new UserResponse
                {
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    Profile_Photo = u.Profile_Photo,
                    FavoriteMovies = u.FavoriteMovies.Select(x => x.Movie.Title).ToList(),
                    Reviews = u.Reviews.Select(x => x.Movie.Title).ToList(),
                }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] int id) 
        {
            var user = service.GetById(id);

            if (user == null)
            {
                return BadRequest("Not found");
            }

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromForm] UserRequest req) 
        {
            User user = new User()
            {
                Email = req.Email,
                Username = req.Username,
                Password = req.Password,
                Profile_Photo = photoService.GetPhotoName(req.Profile_Photo),
                Role = "User"
            };

            service.Save(user);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromForm] UserUpdateRequest req, [FromRoute] int id)
        {
            var user = service.GetById(id);

            if (user == null)
                return BadRequest("Not found");

            if (!string.IsNullOrEmpty(req.Username))
                user.Username = req.Username;

            if (!string.IsNullOrEmpty(req.Email))
                user.Email = req.Email;

            if (!string.IsNullOrEmpty(req.Password))
                user.Password = req.Password;

            if (req.Profile_Photo != null)
                user.Profile_Photo = photoService.GetPhotoName(req.Profile_Photo);

            service.Save(user);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var user = service.GetById(id);

            if (user == null)
                return BadRequest("Not found");

            var loggedUserId = User.FindFirst("loggedUserId")?.Value;
            var loggedUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (loggedUserRole == "User" && loggedUserId != id.ToString())
            {
                return BadRequest();
            }

            photoService.DeletePhoto(user.Profile_Photo);
            service.Delete(user);

            return Ok();
        }
    }
}
