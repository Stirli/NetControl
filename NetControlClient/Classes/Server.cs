

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NetControlClient.Properties;
using NetControlServer.Classes;

namespace NetControlClient.Classes
{
    public partial class Server : INotifyPropertyChanged
    {
        public Server(string host)
        {
            Host = host;
            Size.TryParse(Settings.Default.ScreenshotSize,out var size);
            var pixelWidth = !size.Equals(default(Size))? size.w : 1920;
            var pixelHeight = !size.Equals(default(Size)) ? size.h : 1080;
            Screenshot = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Pbgra32, null);
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