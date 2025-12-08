using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    internal class PhotoService
    {
        private readonly string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "Common", "Assets");

        public PhotoService()
        {
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
        }

        public string SavePhoto(IFormFile photo)
        {

            string fileName = "";

            if (photo != null && photo.Length > 0)
            {
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";

                string filePath = Path.Combine(imageFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

            }
            return $"Assets/{fileName}";
        }
    }
}
