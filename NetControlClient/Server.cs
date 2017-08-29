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
using System.Windows.Media.Imaging;
using NetControlClient.Annotations;
using NetControlClient.Properties;
using Size = System.Windows.Size;


namespace NetControlClient
{
    public class Server : INotifyPropertyChanged
    {
        public Server(string host)
        {
            Host = host;
            var size = Size.Parse(Settings.Default.ScreenshotSize);
            pixelsCount = (int)(size.Width * size.Height);
            wbitmap = new WriteableBitmap((int)size.Width, (int)size.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32, null);
            Refresh();
        }

        public string Host { get; }
        public WriteableBitmap Screenshot => wbitmap;

        public bool IsOnline { get; private set; }

        public void UpdateBackBuffer()
        {
            Runner.IgnoreErr(() =>
            {
                WebRequest req2 = WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc?size={Settings.Default.ScreenshotSize}");

                var resp2 = req2.GetResponse();
                var stream = resp2.GetResponseStream();

                var bmpi = Bitmap.FromStream(stream);
                Byte[] imageArray;


                using (MemoryStream outputStream = new MemoryStream())
                {
                    bmpi.Save(outputStream, ImageFormat.Png);
                    imageArray = outputStream.ToArray();
                }
                wbitmap.FromByteArray(imageArray);
                //List<byte> bytes = new List<byte>();
                //while (true)
                //{
                //    var readByte = stream.ReadByte();
                //    if (readByte == -1) break;
                //    bytes.Add((byte)readByte);
                //}
                //wbitmap.FromByteArray(bytes.ToArray());
            });
        }
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
            OnPropertyChanged(nameof(IsOnline));

            UpdateBackBuffer();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private WriteableBitmap wbitmap;
        private int pixelsCount;
    }
}
