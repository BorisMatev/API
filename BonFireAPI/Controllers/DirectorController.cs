using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Services;
using BonFireAPI.Models.RequestDTOs.Director;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using BonFireAPI.Models.ResponseDTOs.Director;
using System.Linq.Expressions;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DirectorController : BaseController<Director,DirectorService, DirectorRequest, DirectorResponse,DirectorGetRequest,DirectorGetResponse>
    {
        protected override void PopulateEntity(Director forUpdate, DirectorRequest model, out string error)
        {
            error = null;
            forUpdate.Name = model.Name;
            forUpdate.Biography = model.Biography;
        }
        protected override void PopulateResponseEntity(DirectorResponse forUpdate, Director model, out string error)
        {
            error = null;
            forUpdate.Id = model.Id;
            forUpdate.Name = model.Name;
            forUpdate.Biography = model.Biography;
            forUpdate.Movies = model.Movies.Select(x=> x.Title).ToList();
        }

        protected virtual Expression<Func<Actor, bool>> GetFilter(DirectorGetRequest model)
        {
            model.Filter ??= new DirectorGetFilterRequest();

            return
                u => (string.IsNullOrEmpty(model.Filter.Name) || u.Name.Contains(model.Filter.Name));
        }

        protected virtual void PopulateGetResponse(DirectorGetRequest request, DirectorGetResponse response)
        {
            response.Filter = request.Filter;
        }
    }
}
