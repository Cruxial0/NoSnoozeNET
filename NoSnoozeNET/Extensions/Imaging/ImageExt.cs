using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = System.Windows.Controls.Image;

namespace NoSnoozeNET.Extensions.Imaging
{
    public static class ImageExt
    {
        public static System.Drawing.Image ByteArrayToImage(byte[] imageBytes)
        {
            return new ImageConverter().ConvertFrom(imageBytes) as System.Drawing.Image;
        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(imageIn, typeof(byte[]));
            return xByte;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }
    }
}
