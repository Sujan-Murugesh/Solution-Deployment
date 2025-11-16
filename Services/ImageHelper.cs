using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sujan_Solution_Deployer.Services
{
    public static class ImageHelper
    {
        /// Convert image to Base64 string for email embedding
        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            if (image == null)
                return string.Empty;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting image to Base64: {ex.Message}");
                return string.Empty;
            }
        }

        /// Get Base64 string with proper data URI format for email
        public static string GetBase64ImageDataUri(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            var base64 = ImageToBase64(image, format);
            if (string.IsNullOrEmpty(base64))
                return string.Empty;

            // Determine MIME type
            string mimeType = "image/png";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                mimeType = "image/jpeg";
            else if (format.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                mimeType = "image/gif";

            return $"data:{mimeType};base64,{base64}";
        }
    }
}
