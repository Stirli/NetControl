using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NetControlClient.Properties;
using NetControlCommon.Utils;

namespace NetControlClient.Classes
{
    public partial class Server
    {
        public async Task Refresh()
        {
            try
            {
                var isOn = await CheckOnline();
                //OnPropertyChanged(nameof(IsOnline));
                if (!isOn)
                {
                    App.InMainDispatcher(() =>
                    {
                        Screenshot.Lock();
                        Screenshot.Clear(Colors.Fuchsia);
                        Screenshot.FillPolygon(new[] { 0, 0, 10, 0, Screenshot.PixelWidth, Screenshot.PixelHeight - 10, Screenshot.PixelWidth, Screenshot.PixelHeight, 0, 0 },
                            Colors.Black);
                        Screenshot.FillPolygon(
                            new[] { 0, Screenshot.PixelHeight, 10, Screenshot.PixelHeight, Screenshot.PixelWidth, 10, Screenshot.PixelWidth, 0, 0, Screenshot.PixelHeight },
                            Colors.Black);
                        Screenshot.Unlock();
                    });
                }
                else
                {
                    await UpdateBackBuffer();
                }
                IsOnline = isOn;
            }
            catch (WebException e)
            {
                var ee = e;
            }

        }

        public async Task Suspend()
        {
            WebRequest req =
                WebRequest.CreateHttp($"http://{Host}/api/suspend?token={(Host + DateTime.Today).GetHash<SHA256Cng>()}");
            var resp = await req.GetResponseAsync();
            resp.Close();
        }
    }
}
