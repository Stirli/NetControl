using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetControlClient.Classes;

namespace NetControlClient.Windows.Main
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var elem = sender as FrameworkElement;
            var serv = elem.DataContext as Server;
            var win = new ShowImageWindow();
            win.DataContext = serv;
            win.Show();
        }
    }
}