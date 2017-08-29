using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace NetControlClient
{
    public class Server : INotifyPropertyChanged
    {
        public Server(string host)
        {
            Host = host;
            var size = Size.Parse(Settings.Default.ScreenshotSize);
            wbitmap = new WriteableBitmap((int) size.Width, (int) size.Height, 96, 96, System.Windows.Media.PixelFormats.Default, null);
            Screenshot = new BitmapImage();
            Refresh().Wait(1000);
        }

        public string Host { get; }
        public BitmapSource Screenshot { get; private set; }

        public bool IsOnline { get; private set; }

        public async Task Refresh()
        {
            WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/test/echo?mes=message");
            StringBuilder str = new StringBuilder();
            var resp = await req.GetResponseAsync().IgnoreErrAsync();
            var responseStream = resp?.GetResponseStream();
            if (responseStream != null)
                str.Append(new StreamReader(responseStream).ReadToEnd());

            Runner.InMainDispatcher(() => IsOnline = str.ToString().Equals("message"));
            if (resp == null) return;

            WebRequest req2 = WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc?size={Settings.Default.ScreenshotSize}");

            var resp2 = await req2.GetResponseAsync();

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            if (resp2 != null) img.StreamSource = resp2.GetResponseStream();
            img.EndInit();
            Screenshot = img;

            OnPropertyChanged(nameof(IsOnline));
            OnPropertyChanged(nameof(Screenshot));

        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private WriteableBitmap wbitmap;
    }
}
