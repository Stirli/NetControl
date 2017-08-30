using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoozyPainter.Annotations;

namespace WoozyPainter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Bitmap = BitmapFactory.New((int)Width, (int)Height);
            Bitmap.Clear(Colors.White);
            DataContext = this;
        }

        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (Equals(value, _bitmap)) return;
                _bitmap = value;
                OnPropertyChanged();
            }
        }

        private Timer timer;
        private WriteableBitmap _bitmap;

        private Point last= new Point();
        private void Image_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var inputElement = (IInputElement)sender;
            var p = e.GetPosition(inputElement);
            Bitmap.DrawLine((int)Math.Round(last.X), (int)Math.Round(last.Y),(int) Math.Round(p.X), (int)Math.Round(p.Y), Colors.Black);
            last = p;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
