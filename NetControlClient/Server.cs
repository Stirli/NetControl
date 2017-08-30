using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using NetControlClient.Properties;
using NetControlClient.Utils;
using NetControlCommon;
using NetControlCommon.Utils;
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
            WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc?size={Settings.Default.ScreenshotSize}");
            var resp = await req.GetResponseAsync();
            var stream = resp.GetResponseStream();
            int stride;
            byte[] imageArray;
            MakeBitmapSource.FromStream(stream, out imageArray, out stride);
            App.InMainDispatcher(() => { wbitmap.WritePixels(imageArray, stride); });
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
