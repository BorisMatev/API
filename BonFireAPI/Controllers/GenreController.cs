using BonFireAPI.Models.RequestDTOs.Genre;
using BonFireAPI.Models.ResponseDTOs.Genre;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GenreController : BaseController<Genre,GenreService,GenreRequest,GenreResponse>
    {
        protected override void PopulateEntity(Genre forUpdate, GenreRequest model, out string error)
        { 
            error = null;
            forUpdate.Name = model.Name;
        }

        protected override void PopulateResponseEntity(GenreResponse forUpdate, Genre model, out string error)
        { 
            error = null;
            forUpdate.Id = model.Id;
            forUpdate.Name = model.Name;
            forUpdate.Movies = model.Movies.Select(x => x.Movie.Title).ToList();
        }

    }
}
