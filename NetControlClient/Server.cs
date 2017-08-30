using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using NetControlClient.Annotations;
using NetControlClient.Properties;
using NetControlCommon;
using Size = System.Windows.Size;


namespace NetControlClient
{
    public class Server : INotifyPropertyChanged
    {
        public Server(string host)
        {
            Host = host;
            var size = Size.Parse(Settings.Default.ScreenshotSize);
            wbitmap = new WriteableBitmap((int)size.Width, (int)size.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32, null);
            Refresh();
        }

        public string Host { get; }
        public WriteableBitmap Screenshot => wbitmap;

        public bool IsOnline { get; private set; }

        public async Task UpdateBackBuffer()
        {
            WebRequest req2 = WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc?size={Settings.Default.ScreenshotSize}");

            BitmapFrame frame;
            using (var resp2 = await req2.GetResponseAsync())
            {
                var stream = resp2.GetResponseStream();
                frame = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames.First();
            }
            int stride = frame.PixelWidth * frame.Format.BitsPerPixel / 8;
            int offset = 0;
            byte[] imageArray = new byte[frame.PixelHeight * stride];
            frame.CopyPixels(imageArray, stride, offset);
            App.InMainDispatcher(() =>
            {
                wbitmap.Lock();
                wbitmap.WritePixels(new Int32Rect(0, 0, frame.PixelWidth, frame.PixelHeight), imageArray, stride, 0);
                wbitmap.Unlock();
            });
        }
        public async Task Refresh()
        {
            IsOnline = await CheckOnline();
            OnPropertyChanged(nameof(IsOnline));
            if (IsOnline)
                await UpdateBackBuffer();
        }

        private async Task<bool> CheckOnline()
        {
            WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/test/echo?mes=message");
            req.Timeout = 1000;
            StringBuilder str = new StringBuilder();
            var resp = await req.GetResponseAsync().CatchAsync();
            if (resp == null) return false;
            var responseStream = resp?.GetResponseStream();
            if (responseStream != null)
                str.Append(new StreamReader(responseStream).ReadToEnd());

            return str.ToString().Equals("message");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            App.InMainDispatcher(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        private WriteableBitmap wbitmap;
    }
}
