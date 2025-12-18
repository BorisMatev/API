using BonFireAPI.Models.RequestDTOs.Actor;
using BonFireAPI.Models.ResponseDTOs.Actor;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActorController : BaseController<Actor,ActorService, ActorRequest, ActorResponse,ActorGetRequest,ActorGetResponse>
    {
        private PhotoService photoService = new PhotoService();

        protected override void PopulateEntity(Actor forUpdate, ActorRequest model, out string error)
        {
            error = null;
            forUpdate.Name = model.Name;
            forUpdate.Photo = photoService.GetPhotoName(model.Photo);
        }
        protected override void PopulateResponseEntity(ActorResponse forUpdate, Actor model, out string error)
        {
            error = null;
            forUpdate.Id = model.Id;
            forUpdate.Name = model.Name;
            forUpdate.Photo = model.Photo;
            forUpdate.Movies = model.Movies.Select(x => x.Movie.Title).ToList();
        }

        protected virtual Expression<Func<Actor, bool>> GetFilter(ActorGetRequest model)
        {
            model.Filter ??= new ActorGetFilterRequest();

            return
                u =>(string.IsNullOrEmpty(model.Filter.Name) || u.Name.Contains(model.Filter.Name));
        }

        protected virtual void PopulateGetResponse(ActorGetRequest request, ActorGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override void OnDelete(Actor entity,out string error)
        {
            error = null;
            photoService.DeletePhoto(entity.Photo);
        }
    }
}
