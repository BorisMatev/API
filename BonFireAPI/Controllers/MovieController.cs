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
            var result = service.GetAll()
                .Select(m => new ResponseMovies
                {
                    Id = m.Id,
                     Title = m.Title,
                     Release_Date = m.Release_Date,
                     Rating = m.Rating,
                     CoverPath = m.CoverPath,
                     DirectorName = m.Director.Name,
                     Genres = m.Genres.Select(x => x.Genre.Name).ToList(),
                     Actors = m.Actors.Select(x => x.Actor.Name).ToList()
                }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateMovie([FromForm] MovieRequest request)
        {
            DirectorService ds = new DirectorService();
            var director = ds.GetAll().FirstOrDefault(d => d.Name == request.DirectorName);

            PhotoService ps = new PhotoService();

            if (director == null) 
                return BadRequest("Director not found!");

            var movie = new Movie
            {
                Title = request.Title,
                Rating = request.Rating,
                Release_Date = request.Release_Date,
                CoverPath = ps.GetPhotoName(request.Photo),
                DirectorId = director.Id
            };

            service.Save(movie);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromForm] MovieRequest req, [FromRoute] int id)
        {
            var movie = service.GetById(id);

            if (movie == null)
                return BadRequest("Not found");

            movie.Title = req.Title;
            movie.Rating = req.Rating;
            movie.Release_Date = req.Release_Date;

            if (req.Photo != null)
            {
                PhotoService ps = new PhotoService();

                ps.DeletePhoto(movie.CoverPath);

                movie.CoverPath = ps.GetPhotoName(req.Photo);
            }

            service.Save(movie);

            return Ok();
        }

        [HttpGet]
        [Route("add-actor/{movieId}/{actorId}")]
        public IActionResult AddActor([FromRoute]int movieId, [FromRoute] int actorId) 
        {
            MovieService ms = new MovieService();
            ActorService actorService = new ActorService();

            var movie = ms.GetById(movieId);
            var actor = actorService.GetById(actorId);

            if (movie == null || actor == null) 
            {
                return BadRequest();
            }

            if (movie.Actors.Any(ma => ma.ActorId == actor.Id))
            {
                return BadRequest("Actor already added to this movie.");
            }



            movie.Actors.Add(new MovieActor
            {
                MovieId = movie.Id,
                ActorId = actor.Id
            });

            ms.Save(movie);

            return Ok();
        }

        [HttpGet]
        [Route("remove-actor/{movieId}/{actorId}")]
        public IActionResult RemoveActor([FromRoute] int movieId, [FromRoute] int actorId)
        {
            MovieService ms = new MovieService();
            ActorService actorService = new ActorService();

            var movie = ms.GetById(movieId);
            var actor = actorService.GetById(actorId);

            if (movie == null || actor == null)
            {
                return BadRequest();
            }

            var movieActor = movie.Actors.FirstOrDefault(a => a.ActorId == actor.Id);

            if (movieActor == null)
            {
                return BadRequest("Actor is not added to this movie.");
            }

            movie.Actors.Remove(movieActor);
            ms.Save(movie);

            return Ok();
        }

    }
}
