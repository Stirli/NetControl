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
using System.Windows.Media.Imaging;
using NetControlClient.Annotations;

namespace NetControlClient
{
    public class Client:INotifyPropertyChanged
    {
        public Client(string host)
        {
            Host = host;
        }

        public string Host { get; }
        public BitmapSource Screenshot
        {
            get
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/api/prtsc");
                var resp = req.GetResponse();
                img.StreamSource = resp.GetResponseStream();
                img.EndInit();
                
                return img;
            }
        }

        public bool IsOnline
        {
            get
            {
                WebRequest req = WebRequest.CreateHttp($"http://{Host}:8080/test/echo?mes=message");
                string str = "";
                try
                {
                    var resp = req.GetResponse();
                    str = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    resp.Close();
                }
                catch (Exception ex)
                {
                    // ignored
                    var s = ex.Message;
                }
                return str.Equals("message");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
