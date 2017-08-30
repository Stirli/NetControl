using System;
using System.Data.OracleClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Size = System.Drawing.Size;

namespace NetControlServer.Classes
{
    public static class ScreenCapturer
    {
        static readonly int left = Screen.AllScreens.Min(screen => screen.Bounds.X);
        static readonly int top = Screen.AllScreens.Min(screen => screen.Bounds.Y);
        static readonly int right = Screen.AllScreens.Max(screen => screen.Bounds.X + screen.Bounds.Width);
        static readonly int bottom = Screen.AllScreens.Max(screen => screen.Bounds.Y + screen.Bounds.Height);
        static readonly int width = right - left;
        static readonly int height = bottom - top;
        static Bitmap original = new Bitmap(width, height);
        private static Graphics originalGraphics = Graphics.FromImage(original);

        public static byte[] Take()
        {
            return Take(width, height);
        }
        public static byte[] Take(int newWidth, int newHeight)
        {
            using (Bitmap resultBmp = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(resultBmp))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.SmoothingMode = SmoothingMode.None;
                    lock (originalGraphics)
                    {
                        originalGraphics.CopyFromScreen(left, top, 0, 0, new Size(width, height));
                        graphics.DrawImage(original, 0, 0, newWidth, newHeight);
                        Byte[] resultImageArray;
                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            resultBmp.Save(outputStream, ImageFormat.Png);
                            resultImageArray = outputStream.ToArray();
                        }
                        return resultImageArray;
                    }
                }
            }
        }
    }
}
