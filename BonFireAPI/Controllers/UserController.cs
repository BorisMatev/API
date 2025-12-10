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

            return Ok(service.GetAll());
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
                Role = "User"
            };

            service.CreateUser(user, req.Profile_Photo);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromForm] UserRequest req, [FromRoute] int id)
        {
            var user = service.GetById(id);

            if (user == null)
                return BadRequest("Not found");

            user.Username = req.Username;
            user.Role = req.Role;
            user.Email = req.Email;
            user.Password = req.Password;

            service.CreateUser(user, req.Profile_Photo);

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

            service.Delete(user);

            return Ok();
        }
    }
}
