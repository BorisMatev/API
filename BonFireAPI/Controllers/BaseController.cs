using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<E, EService, ERequest, EResponse> : ControllerBase
        where E : BaseEntity, new()
        where EService : BaseService<E>, new()
        where EResponse : class, new()
        where ERequest : class
    {
        EService service = new EService();

        protected virtual void PopulateEntity(E forUpdate, ERequest model, out string error)
        { error = null; }

        protected virtual void PopulateResponseEntity(EResponse forUpdate, E model, out string error)
        { error = null; }

        protected virtual void OnDelete(E entity, out string error)
        { error = null; }

        [HttpGet]
        public IActionResult GetAll()
        {
            string error;

            var resp = service.GetAll().Select(x => {

                EResponse item = new EResponse();
                PopulateResponseEntity(item, x, out error);
                return item;

             }).ToList();

            return Ok(resp);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var model = service.GetById(id);
            string error;

            if (model == null)
            {
                return BadRequest("Not found");
            }

            EResponse resp = new EResponse();

            PopulateResponseEntity(resp, model, out error);


            return Ok(resp);
        }

        [HttpPost]
        public IActionResult Create([FromForm] ERequest req)
        {
            var entity = new E(){};
            string error;

            PopulateEntity(entity, req, out error);

            if (error != null)
            {
                return BadRequest(error);
            }

            service.Save(entity);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromBody] ERequest req, [FromRoute] int id)
        {
            var entity = service.GetById(id);
            string error;

            if (entity == null)
                return BadRequest("Not found");

            PopulateEntity(entity, req,out error);

            service.Save(entity);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var entity = service.GetById(id);

            if (entity == null)
                return BadRequest("Not found");

            if (typeof(E).Name == "User" || typeof(E).Name == "Actor" || typeof(E).Name == "Movie")
            {
                string error;

                OnDelete(entity,out error);

                if (error != null)
                {
                    return BadRequest(error);
                }
            }

            service.Delete(entity);

            return Ok();
        }
    }
}
