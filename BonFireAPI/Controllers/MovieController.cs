using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Services;
using Common.Entities;
using BonFireAPI.Models.ResponseDTOs.Movies;
using BonFireAPI.Models.RequestDTOs.Movie;
using Microsoft.AspNetCore.Authorization;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private MovieService service = new MovieService();

        [HttpGet]
        public IActionResult GetAll()
        {

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var result = service.GetAll()
                .Select(m => new ResponseMovies
                {
                     Title = m.Title,
                     Release_Date = m.Release_Date,
                     Rating = m.Rating,
                     CoverPath = $"{baseUrl}/{m.CoverPath}",
                     DirectorName = m.Director.Name,
                     Genres = m.Genres.Select(x => x.Genre.Name).ToList(),
                     Actors = m.Actors.Select(x => x.Actor.Name).ToList()
                }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateMovie([FromForm] RequestMovie reques)
        {
            DirectorService ds = new DirectorService();
            var director = ds.GetAll().FirstOrDefault(d => d.Name == reques.DirectorName);

            if (director == null) 
                return BadRequest("Director not found!");

            var movie = new Movie
            {
                Title = reques.Title,
                Rating = reques.Rating,
                Release_Date = reques.Release_Date,
                CoverPath = "",
                DirectorId = director.Id
            };

            var created = service.CreateMovie(movie, reques.Photo);

            return Ok(created);
        }
    }
}
