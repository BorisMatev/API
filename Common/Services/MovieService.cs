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
        private readonly string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "Common", "Assets");

        public MovieService()
        {
            
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
        }

        public Movie CreateMovie(Movie movie, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                // generate safe and unique file name
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";

                string filePath = Path.Combine(imageFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                movie.CoverPath = $"Assets/{fileName}";
            }

            Save(movie); // uses BaseService

            return movie;
        }
    }
}
