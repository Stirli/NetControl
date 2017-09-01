using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetControlClient.Classes;
using NetControlClient.Windows.Main.ViewModels;

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

        private async void SuspendBtnClick(object sender, RoutedEventArgs e)
        {
            if (!((sender as FrameworkElement)?.DataContext is Server serv)) return;
            var res = MessageBox.Show(this, $"Вы действительно хотите выключить '{serv.Host}'?", "Выключение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                await serv.Suspend();
            }
        }

        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show(this, $"Вы действительно хотите выключить все машины?", "Выключение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                if ((sender as FrameworkElement)?.DataContext is MainViewModel mvm)
                {
                    foreach (var server in mvm.Servers)
                    {
                        await server.Suspend();
                    }
                }
            }

        }
    }
}