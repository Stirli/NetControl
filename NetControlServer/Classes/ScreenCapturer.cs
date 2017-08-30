using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NetControlServer.Classes
{
    public static class ScreenCapturer
    {
        private static readonly int left = Screen.AllScreens.Min(screen => screen.Bounds.X);
        private static readonly int top = Screen.AllScreens.Min(screen => screen.Bounds.Y);
        private static readonly int right = Screen.AllScreens.Max(screen => screen.Bounds.X + screen.Bounds.Width);
        private static readonly int bottom = Screen.AllScreens.Max(screen => screen.Bounds.Y + screen.Bounds.Height);
        private static readonly int width = right - left;
        private static readonly int height = bottom - top;
        private static readonly Bitmap original = new Bitmap(width, height);
        private static readonly Graphics originalGraphics = Graphics.FromImage(original);

        public static byte[] Take()
        {
            return Take(width, height);
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
                        originalGraphics.CopyFromScreen(left, top, 0, 0, new Size(width, height));
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