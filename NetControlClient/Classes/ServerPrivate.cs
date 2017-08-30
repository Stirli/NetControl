using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NetControlClient.Properties;
using NetControlClient.Utils;
using NetControlCommon.Utils;

namespace NetControlClient.Classes
{
    public partial class Server : INotifyPropertyChanged
    {
        private bool _isOnline;

        private async Task UpdateBackBuffer()
        {
            WebRequest req =
                WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc?size={Settings.Default.ScreenshotSize}");
            var resp = await req.GetResponseAsync();
            var stream = resp.GetResponseStream();
            int stride;
            byte[] imageArray;
            MakeBitmapSource.FromStream(stream, out imageArray, out stride);
            App.InMainDispatcher(() => { Screenshot.WritePixels(imageArray, stride); });
        }


        private async Task<bool> CheckOnline()
        {
            WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/test/echo?mes=message");
            req.Timeout = 1000;
            var str = new StringBuilder();
            var resp = await req.GetResponseAsync().CatchAsync();
            if (resp == null) return false;
            var responseStream = resp.GetResponseStream();
            if (responseStream != null)
                str.Append(new StreamReader(responseStream).ReadToEnd());

            return str.ToString().Equals("message");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            App.InMainDispatcher(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
