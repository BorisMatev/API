using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Common.Services
{
    public class MovieService : BaseService<Movie>
    {
        public Movie CreateMovie(Movie movie, IFormFile photo)
        {
            PhotoService service = new PhotoService();
            
            string path = service.SavePhoto(photo);

            movie.CoverPath = path;

            Save(movie);

            return movie;
        }
    }
}
