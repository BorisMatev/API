using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Services;
using Common.Entities;
using BonFireAPI.Models.ResponseDTOs.Movies;
using BonFireAPI.Models.RequestDTOs.Movie;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Linq;
using BonFireAPI.Models.ResponseDTOs;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : BaseController<Movie, MovieService, MovieRequest, MovieResponse,MovieGetRequest,MovieGetResponse>
    {
        private PhotoService photoService = new PhotoService();
        protected override void PopulateEntity(Movie forUpdate, MovieRequest model, out string error)
        {
            error = null;

            DirectorService ds = new DirectorService();
            var director = ds.GetAll().FirstOrDefault(d => d.Name == model.DirectorName);


            if (director == null)
            {
                error = "Director not found!";
                return;
            }

            if (forUpdate.CoverPath != null)
            {
                photoService.DeletePhoto(forUpdate.CoverPath);
            }

            forUpdate.CoverPath = photoService.GetPhotoName(model.Photo);
            forUpdate.Title = model.Title;
            forUpdate.Release_Date = model.Release_Date;
            forUpdate.DirectorId = director.Id;
        }

        protected override void PopulateResponseEntity(MovieResponse forUpdate, Movie model, out string error)
        {
            error = null;
            forUpdate.Id = model.Id;
            forUpdate.Title = model.Title;
            forUpdate.Release_Date = model.Release_Date;
            forUpdate.Rating = model.Rating;
            forUpdate.CoverPath = model.CoverPath;
            forUpdate.DirectorName = model.Director.Name;
            forUpdate.Genres = model.Genres.Select(x => x.Genre.Name).ToList();
            forUpdate.Actors = model.Actors.Select(x => x.Actor.Name).ToList();
            forUpdate.Reviews = model.Reviews.Select(x => new ReviewResponse
            {
                Comment = x.Comment,
                Id = x.Id,
                UserName = x.User.Username,
                Rating = x.Rating,
                UserId = x.UserId
            }).ToList();
        }

        protected override Expression<Func<Movie, bool>> GetFilter(MovieGetRequest model)
        {
            model.Filter ??= new MovieGetFilterRequest();

            return
                u =>
                    (string.IsNullOrEmpty(model.Filter.Genre) || u.Genres.Any(x => x.Genre.Name.Contains(model.Filter.Genre))) &&
                    (string.IsNullOrEmpty(model.Filter.Title) || u.Title.Contains(model.Filter.Title)) &&
                    (string.IsNullOrEmpty(model.Filter.Release_Date) || u.Release_Date.Contains(model.Filter.Release_Date)) &&
                    (model.Filter.DirectorId == null || u.DirectorId == model.Filter.DirectorId) &&
                    (model.Filter.Rating == null || u.Rating == model.Filter.Rating) &&
                    (string.IsNullOrEmpty(model.Filter.Actor) || u.Actors.Any(x => x.Actor.Name.Contains(model.Filter.Actor)));
        }

        protected override void PopulateGetResponse(MovieGetRequest request, MovieGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override void OnDelete(Movie entity, out string error)
        { 
            error = null;
            photoService.DeletePhoto(entity.CoverPath);
        }

        [HttpGet]
        [Route("add-actor/{movieId}/{actorId}")]
        public IActionResult AddActor([FromRoute]int movieId, [FromRoute] int actorId) 
        {
            MovieService movieService = new MovieService();
            ActorService actorService = new ActorService();

            var movie = movieService.GetById(movieId);
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

            movieService.Save(movie);
            return Ok();
        }

        [HttpGet]
        [Route("remove-actor/{movieId}/{actorId}")]
        public IActionResult RemoveActor([FromRoute] int movieId, [FromRoute] int actorId)
        {
            MovieService movieService = new MovieService();
            ActorService actorService = new ActorService();

            var movie = movieService.GetById(movieId);
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
            movieService.Save(movie);

            return Ok();
        }

        [HttpGet]
        [Route("add-genre/{movieId}/{genreId}")]
        public IActionResult AddGenre([FromRoute]int movieId, [FromRoute]int genreId) 
        {
            MovieService movieService = new MovieService();
            GenreService genreService = new GenreService();

            var movie = movieService.GetById(movieId);
            var genre = genreService.GetById(genreId);
            if (movie == null || genre == null)
            {
                return BadRequest("Not found");
            }

            if (movie.Genres.Any(m => m.GenreId == genre.Id))
            {
                return BadRequest("Genre already added to this movie.");
            }

            movie.Genres.Add(new MovieGenre 
            {
                GenreId = genre.Id,
                MovieId = movie.Id
            });

            movieService.Save(movie);
            return Ok();
        }

        [HttpGet]
        [Route("remove-genre/{movieId}/{genreId}")]
        public IActionResult RemoveGenre([FromRoute] int movieId, [FromRoute] int genreId)
        {
            MovieService movieService = new MovieService();
            GenreService genreService = new GenreService();

            var movie = movieService.GetById(movieId);
            var genre = genreService.GetById(genreId);
            if (movie == null || genre == null)
            {
                return BadRequest("Not found");
            }

            var movieGenre = movie.Genres.FirstOrDefault(m => m.GenreId == genre.Id);

            if (movieGenre == null)
            {
                return BadRequest("Actor is not added to this movie.");
            }

            movie.Genres.Remove(movieGenre);
            movieService.Save(movie);
            return Ok();
        }

    }
}
