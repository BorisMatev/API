using BonFireAPI.Models.RequestDTOs.Auth;
using BonFireAPI.Services;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(AuthRequest req) 
        {

            UserService s = new UserService();
            var user = s.GetAll()
                            .FirstOrDefault(x =>
                                x.Username == req.Username &&
                                x.Password == req.Password);
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            TokenService tokenService = new TokenService();

            string token = tokenService.GenerateToken(user);

            return Ok(token);
        } 
    }
}
