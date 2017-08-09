using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace NetControlClient
{
    public static class ScreenCapturer
    {
        public static byte[] Take2()
        {
            int screenWidth = Convert.ToInt32(SystemParameters.VirtualScreenWidth);
            int screenHeight = Convert.ToInt32(SystemParameters.VirtualScreenHeight);
            int screenLeft = Convert.ToInt32(SystemParameters.VirtualScreenLeft);
            int screenTop = Convert.ToInt32(SystemParameters.VirtualScreenTop);

            RenderTargetBitmap renderTarget =
                new RenderTargetBitmap(screenWidth, screenHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush();

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.PushTransform(new ScaleTransform(1, 1));
                drawingContext.DrawRectangle(sourceBrush, null,
                    new Rect(new Point(0, 0), new Point(screenWidth, screenHeight)));
            }
            renderTarget.Render(drawingVisual);

            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            Byte[] _imageArray;

            using (MemoryStream outputStream = new MemoryStream())
            {
                pngEncoder.Save(outputStream);
                _imageArray = outputStream.ToArray();
            }

            return _imageArray;
        }

        public static byte[] Take()
        {
            var left = Screen.AllScreens.Min(screen => screen.Bounds.X);
            var top = Screen.AllScreens.Min(screen => screen.Bounds.Y);
            var right = Screen.AllScreens.Max(screen => screen.Bounds.X + screen.Bounds.Width);
            var bottom = Screen.AllScreens.Max(screen => screen.Bounds.Y + screen.Bounds.Height);
            var width = right - left;
            var height = bottom - top;

            using (var screenBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {

                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));

                    Byte[] _imageArray;


                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        screenBmp.Save(outputStream, ImageFormat.Png);
                        _imageArray = outputStream.ToArray();
                    }

                    return _imageArray;
                }
            }
        }
    }
}
