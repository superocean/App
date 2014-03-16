using ImageResizer;
using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace DealSearchEngine.Utility
{
    public class ImageUtilities
    {
        public string ImageCropAndToBase64StringPNG(string url, int width, int height)
        {
            string fullFilePath = HttpContext.Current.Server.MapPath(url);
            var resizeSettings = this.instantiateResizeSettings(width, height);
            var resizedImage = ImageBuilder.Current.Build(fullFilePath, resizeSettings);
            return "data:image/png;base64," + ConvertImageToBase64StringPNG(resizedImage);
        }

        private ResizeSettings instantiateResizeSettings(int width, int height)
        {
            var queryString = string.Format("maxwidth={0}&maxheight={1}&quality=90", width, height);
            return new ResizeSettings(queryString);
        }

        /// <summary>
        /// Converts the image to base64 string.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        private string ConvertImageToBase64String(Bitmap imageBitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Convert Image to byte[]
                imageBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = memoryStream.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private string ConvertImageToBase64StringPNG(Bitmap imageBitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Convert Image to byte[]
                imageBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = memoryStream.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}