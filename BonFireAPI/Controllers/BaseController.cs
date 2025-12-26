using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Shared;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<E, EService, ERequest, EResponse, EGetRequest, EGetResponse> : ControllerBase
        where E : BaseEntity, new()
        where EService : BaseService<E>, new()
        where EResponse : class, new()
        where ERequest : class
        where EGetRequest : BaseGetRequest, new()
        where EGetResponse : BaseGetResponse<EResponse>, new()
    {
        EService service = new EService();

        protected virtual void PopulateEntity(E forUpdate, ERequest model, out string error)
        { error = null; }

        protected virtual void PopulateResponseEntity(EResponse forUpdate, E model, out string error)
        { error = null; }

        protected virtual Expression<Func<E, bool>> GetFilter(EGetRequest model)
        { return null; }

        protected virtual void PopulateGetResponse(EGetRequest request, EGetResponse response)
        { }

        protected virtual void OnDelete(E entity, out string error)
        { error = null; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] EGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                    ? 10
                                    : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);

            EService service = new EService();

            Expression<Func<E, bool>> filter = GetFilter(model);

            var response = new EGetResponse();

            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;

            response.OrderBy = model.OrderBy;
            response.SortAsc = model.SortAsc;

            PopulateGetResponse(model, response);

            response.Pager.Count = service.Count(filter);
            var entities = service.GetAll(
                                        filter,
                                        model.OrderBy,
                                        model.SortAsc,
                                        model.Pager.Page,
                                        model.Pager.PageSize);

            response.Items = new List<EResponse>();

            foreach (var entity in entities)
            {
                var resp = new EResponse();
                PopulateResponseEntity(resp, entity, out string error);
                response.Items.Add(resp);
            }

            return Ok(response);
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
        public IActionResult Update([FromForm] ERequest req, [FromRoute] int id)
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
