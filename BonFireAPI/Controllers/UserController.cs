using BonFireAPI.Models.ResponseDTOs;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserService service = new UserService();

        [HttpGet]
        public IActionResult GetAll() 
        {
            return Ok(service.GetAll());
        }
    }
}
