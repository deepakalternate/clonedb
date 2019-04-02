using CloneDB.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloneDB.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageWriter _imageWriter;

        public ImageController(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }
        
        [HttpPost]
        [Route("api/image/upload")]
        [Consumes("multipart/form-data")]
        public ActionResult<string> SaveImage([FromForm]IFormFile file)
        {
            if (file != null)
            {
                string fileName = _imageWriter.UploadImage(file);

                if (!string.IsNullOrEmpty(fileName))
                {
                    return Ok(fileName);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}