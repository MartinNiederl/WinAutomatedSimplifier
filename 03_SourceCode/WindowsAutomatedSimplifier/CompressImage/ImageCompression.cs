using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WindowsAutomatedSimplifier.CompressImage
{
    internal class ImageCompression
    {
        private Image _image;
        private readonly Bitmap _bitmap;
        public ImageCompression(string imagePath)
        {
            _bitmap = new Bitmap(_image = Image.FromFile(imagePath));
            GetAllColorsOfImage();
        }

        public static void CompressJPG(byte[] inputBytes)
        {
            const int jpegQuality = 50;
            using (var inputStream = new MemoryStream(inputBytes))
            {
                Image image = Image.FromStream(inputStream);
                ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters encoderParameters = new EncoderParameters(1) {
                    Param = {[0] = new EncoderParameter(Encoder.Quality, jpegQuality)}
                };

                byte[] outputBytes;
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, jpegEncoder, encoderParameters);
                    outputBytes = outputStream.ToArray();
                }
            }
        }

        public void GetAllColorsOfImage()
        {
            int[] colors = new int[_bitmap.Size.Height * _bitmap.Size.Width];
            for (int index = 0, y = 0; y < _bitmap.Size.Height; y++)
            {
                for (int x = 0; x < _bitmap.Size.Width; x++, index++)
                {
                    colors[index] = _bitmap.GetPixel(x, y).ToArgb();
                }
            }
           
        }
        
    }
}
