using System.Linq;
using System.Text;

namespace CloneDB.Utilities
{
    public class ImageUtility
    {
        /// <summary>
        /// Enum that stores values for all image formats
        /// </summary>
        public enum ImageFormat
        {
            bmp = 1,
            jpeg = 2,
            gif = 3,
            tiff = 4,
            png = 5,
            Unknown = 6
        }
        
        /// <summary>
        /// Function that checks the signature of the file to see what format it belongs to
        /// Referenced from: https://www.codeproject.com/Articles/1256591/Upload-Image-to-NET-Core-2-1-API
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");             // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");            // GIF
            var png = new byte[] { 137, 80, 78, 71 };               // PNG
            var tiff = new byte[] { 73, 73, 42 };                   // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                  // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };           // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };          // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.Unknown;
        }

    }
}