using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CloneDB.Utilities
{
    public class ImageWriter : IImageWriter
    {
        public string UploadImage(IFormFile file)
        {
            string fileName = "";
            
            if (IsImageFile(file))
            {
                fileName = WriteFile(file);
            }

            return fileName;
        }

        private bool IsImageFile(IFormFile file)
        {
            byte[] fileBytes;
            bool isImageFile = false;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            isImageFile = ImageUtility.GetImageFormat(fileBytes) != ImageUtility.ImageFormat.Unknown;

            return isImageFile;
        }
        
        
        private string WriteFile(IFormFile file)
        {
            string fileName;
            
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; 
                //for the file due to security reasons.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(bits);
                }
            }
            catch (Exception e)
            {
                fileName = "";
                Console.WriteLine(e);
            }

            return fileName;
        }
    }
}