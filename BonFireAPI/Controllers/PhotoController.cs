using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBase
    {

        [HttpGet("{filename}")]
        public IActionResult GetPhoto([FromRoute] string filename) 
        {
            var path = Path.Combine("../Common/Assets/", filename);

            if (!System.IO.File.Exists(path))
                return NotFound("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(path);

            var fileExtension = Path.GetExtension(filename).ToLower();
            string mimeType = fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            return File(fileBytes, mimeType);
        }
    }
}
