using BonFireAPI.Models.RequestDTOs.Actor;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActorController : ControllerBase
    {
        private ActorService service = new ActorService();
        private PhotoService photoService = new PhotoService();

        [HttpGet]
        public IActionResult GetAll()
        {

            var response = service.GetAll()
                .Select(x => new Actor 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Photo = x.Photo
                });

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
        public IActionResult Create([FromForm] ActorRequest req)
        {
            var actor = new Actor()
            {
                Name = req.Name,
                Photo = photoService.GetPhotoName(req.Photo)
            };

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromForm] ActorRequest req, [FromRoute] int id)
        {
            var actor = service.GetById(id);

            if (actor == null)
                return BadRequest("Not found");

            actor.Name = req.Name;

            photoService.GetPhotoName(req.Photo);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var actor = service.GetById(id);

            if (actor == null)
                return BadRequest("Not found");

            service.Delete(actor);

            return Ok();
        }
    }
}
