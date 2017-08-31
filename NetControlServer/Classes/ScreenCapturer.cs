using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace NetControlServer.Classes
{
    public static class ScreenCapturer
    {

        public static readonly Rectangle rect = GetScreenRect();
        private static readonly Bitmap original = new Bitmap(rect.Width, rect.Height);
        private static readonly Graphics originalGraphics = Graphics.FromImage(original);

        public static byte[] Take()
        {
            return Take(rect.Width, rect.Height);
        }

        public static Rectangle GetScreenRect()
        {
            return Screen.AllScreens.First(s => s.Primary).Bounds;
        }

        public static byte[] Take(int newWidth, int newHeight)
        {
            using (var resultBmp = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(resultBmp))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.SmoothingMode = SmoothingMode.None;
                    lock (originalGraphics)
                    {
                        originalGraphics.CopyFromScreen(rect.Location, new Point(), rect.Size);
                        graphics.DrawImage(original, 0, 0, newWidth, newHeight);
                        byte[] resultImageArray;
                        using (var outputStream = new MemoryStream())
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