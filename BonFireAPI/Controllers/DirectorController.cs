using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Services;
using BonFireAPI.Models.RequestDTOs.Director;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DirectorController : ControllerBase
    {
        private DirectorService service = new DirectorService();

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = service.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] int id) 
        {
            var response = service.GetById(id);

            if (response == null)
            {
                return BadRequest("Not found");
            }

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(ActorRequest req) 
        {
            var director = new Director()
            {
                Name = req.Name,
                Biography = req.Biography,
                Movies = new List<Movie>()
            };

            service.Save(director);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(ActorRequest req, [FromRoute] int id)
        {
            var director = service.GetById(id);

            if (director == null)
                return BadRequest("Not found");

            director.Name = req.Name;
            director.Biography = req.Biography;

            service.Save(director);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var director = service.GetById(id);

            if (director == null)
                return BadRequest("Not found");

            service.Delete(director);

            return Ok();
        }
    }
}
