using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NetControlClient.Properties;
using NetControlClient.Utils;
using NetControlCommon.Utils;

namespace NetControlClient.Classes
{
    public partial class Server : INotifyPropertyChanged
    {
        public Server(string host)
        {
            Host = host;
            var size = Size.Parse(Settings.Default.ScreenshotSize);
            Screenshot = new WriteableBitmap((int) size.Width, (int) size.Height, 96, 96, PixelFormats.Pbgra32, null);
            Refresh();
        }

        public string Host { get; }
        public WriteableBitmap Screenshot { get; }

        public bool IsOnline
        {
            get { return _isOnline; }
            private set
            {
                if (_isOnline.Equals(value)) return;
                _isOnline = value;
                OnPropertyChanged();
            }
        }
    }
}