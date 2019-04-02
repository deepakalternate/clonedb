using Microsoft.AspNetCore.Http;

namespace CloneDB.Utilities
{
    public interface IImageWriter
    {
        string UploadImage(IFormFile file);
    }
}