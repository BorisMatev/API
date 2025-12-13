using BonFireAPI.Models.RequestDTOs.Actor;
using BonFireAPI.Models.ResponseDTOs.Actor;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActorController : BaseController<Actor,ActorService, ActorRequest, ActorResponse>
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

        protected override void OnDelete(Actor entity,out string error)
        {
            error = null;
            photoService.DeletePhoto(entity.Photo);
        }
    }
}
