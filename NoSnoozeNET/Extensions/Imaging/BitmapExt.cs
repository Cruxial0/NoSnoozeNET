using ImageMagick;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NoSnoozeNET.Extensions.Imaging
{
    public static class BitmapExt
    {
        public static Bitmap ColorReplace(this Image inputImage, int tolerance, Color oldColor, Color NewColor)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);

            Graphics G = Graphics.FromImage(outputImage);
            G.DrawImage(inputImage, 0, 0);
            Rectangle rect = new Rectangle(0, 0, outputImage.Width, outputImage.Height);

            for (Int32 y = 0; y < outputImage.Height; y++)
                for (Int32 x = 0; x < outputImage.Width; x++)
                {
                    Color PixelColor = outputImage.GetPixel(x, y);
                    if (PixelColor.R > oldColor.R - tolerance && PixelColor.R < oldColor.R + tolerance && PixelColor.G > oldColor.G - tolerance && PixelColor.G < oldColor.G + tolerance && PixelColor.B > oldColor.B - tolerance && PixelColor.B < oldColor.B + tolerance)
                    {
                        int RColorDiff = oldColor.R - PixelColor.R;
                        int GColorDiff = oldColor.G - PixelColor.G;
                        int BColorDiff = oldColor.B - PixelColor.B;

                        if (PixelColor.R > oldColor.R) RColorDiff = NewColor.R + RColorDiff;
                        else RColorDiff = NewColor.R - RColorDiff;
                        if (RColorDiff > 255) RColorDiff = 255;
                        if (RColorDiff < 0) RColorDiff = 0;
                        if (PixelColor.G > oldColor.G) GColorDiff = NewColor.G + GColorDiff;
                        else GColorDiff = NewColor.G - GColorDiff;
                        if (GColorDiff > 255) GColorDiff = 255;
                        if (GColorDiff < 0) GColorDiff = 0;
                        if (PixelColor.B > oldColor.B) BColorDiff = NewColor.B + BColorDiff;
                        else BColorDiff = NewColor.B - BColorDiff;
                        if (BColorDiff > 255) BColorDiff = 255;
                        if (BColorDiff < 0) BColorDiff = 0;

                        outputImage.SetPixel(x, y, Color.FromArgb(RColorDiff, GColorDiff, BColorDiff));
                    }
                }

            return outputImage;
        }

        public static Bitmap FastColorReplace(this Bitmap inputImage, Color oldColor, Color newColor)
        {
            var m = new MagickFactory();
            MagickImage image = ToMagickImage(inputImage) as MagickImage;

            image.ColorFuzz = new Percentage(6);
            image.Opaque(MagickColor.FromRgb((byte)oldColor.R, (byte)oldColor.G, (byte)oldColor.B),
                MagickColor.FromRgb(newColor.R,
                    newColor.G,
                    newColor.B));
            return image.ToBitmap(fmt: MagickFormat.Png);
        }

        public static Bitmap ToBitmap(this MagickImage mimg, MagickFormat fmt = MagickFormat.Png24)
        {
            Bitmap bmp = null;
            using (MemoryStream ms = new MemoryStream())
            {
                mimg.Write(ms, fmt);
                ms.Position = 0;
                bmp = (Bitmap)Bitmap.FromStream(ms);
            }
            return bmp;
        }

        public static IMagickImage ToMagickImage(this Bitmap bmp)
        {
            IMagickImage img = null;
            MagickFactory f = new MagickFactory();
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                img = new MagickImage(f.Image.Create(ms));
            }
            return img;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }


        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static Bitmap BitmapImageToBitmap(this BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
