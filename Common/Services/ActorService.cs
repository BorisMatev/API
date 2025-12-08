using Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ActorService : BaseService<Actor>
    {
        public Actor CreateActor(Actor actor, IFormFile photo)
        {
            PhotoService service = new PhotoService();

            string path = service.SavePhoto(photo);

             actor.Photo = path;

            Save(actor);

            return actor;
        }
    }
}
