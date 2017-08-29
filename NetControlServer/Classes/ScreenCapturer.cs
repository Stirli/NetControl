using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Size = System.Windows.Size;

namespace NetControlServer.Classes
{
    public static class ScreenCapturer
    {
        public static byte[] Take(Size size = default(Size))
        {
            var left = Screen.AllScreens.Min(screen => screen.Bounds.X);
            var top = Screen.AllScreens.Min(screen => screen.Bounds.Y);
            var right = Screen.AllScreens.Max(screen => screen.Bounds.X + screen.Bounds.Width);
            var bottom = Screen.AllScreens.Max(screen => screen.Bounds.Y + screen.Bounds.Height);
            var width = right - left;
            var height = bottom - top;

            using (var screenBmp = new Bitmap((int)(size != default(Size) ? size.Width : width), (int)(size != default(Size) ? size.Height : height), PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    if (size != default(Size))
                    {
                        bmpGraphics.ScaleTransform((float)(size.Width / width), (float)(size.Height / height));
                    }
                    bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));
                    Byte[] imageArray;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        screenBmp.Save(outputStream, ImageFormat.Png);
                        imageArray = outputStream.ToArray();
                    }

                    return imageArray;
                }
            }
        }
    }
}
