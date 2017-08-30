using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NetControlClient.Utils
{
    public static class MakeBitmapSource
    {
        public static void WritePixels(this WriteableBitmap wbitmap, byte[] imageArray, int stride)
        {
            wbitmap.Lock();
            wbitmap.WritePixels(new Int32Rect(0, 0, wbitmap.PixelWidth, wbitmap.PixelHeight), imageArray, stride, 0);
            wbitmap.Unlock();
        }

        public static byte[] CopyPixels(this BitmapSource frame, out int stride)
        {
            stride = frame.PixelWidth * frame.Format.BitsPerPixel / 8;
            int offset = 0;
            byte[] imageArray = new byte[frame.PixelHeight * stride];
            frame.CopyPixels(imageArray, stride, offset);
            return imageArray;
        }

        public static BitmapSource FromStream(Stream stream)
        {
            BitmapSource frame =
                new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames.Last();
            return frame;
        }

        public static void FromStream(Stream stream, out byte[] imageArray, out int stride)
        {
            var frame = MakeBitmapSource.FromStream(stream);
            imageArray = frame.CopyPixels(out stride);
        }
    }
}
