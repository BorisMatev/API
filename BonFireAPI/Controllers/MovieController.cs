using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Services;
using Common.Entities;
using BonFireAPI.Models.RequestDTOs;
using BonFireAPI.Models.ResponseDTOs.Movies;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        MovieService service = new MovieService();
        [HttpGet]
        public ActionResult<List<ResponseMovies>> GetAll()
        {
            var movies = service.GetAll();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var result = movies.Select(m => new ResponseMovies
            {
                Title = m.Title,
                Release_Date = m.Release_Date,
                Rating = m.Rating,
                PhotoPath = $"{baseUrl}/Assets/{Path.GetFileName(m.CoverPath)}"
            }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateMovie([FromForm] RequestMovie reques)
        {

            var movie = new Movie 
            {
                Title = reques.Title,
                Rating = reques.Rating,
                Release_Date = reques.Release_Date,
                CoverPath = ""
            };

            var created = service.CreateMovie(movie, reques.Photo);

            return Ok(created);
        }
    }
}
