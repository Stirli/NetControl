using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetControlClient.Classes;
using NetControlClient.Properties;
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

        private async void SuspendMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show(this, "Вы действительно хотите выключить все машины?", "Выключение", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        private bool adding;
        private async void AddCompMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (adding) return;
            adding = true;
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 7878);
                listener.Start();
                var client = await listener.AcceptTcpClientAsync();
                var ipEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                if (ipEndPoint != null)
                {
                    var host = ipEndPoint.Address + ":8080";
                    if (MessageBox.Show(this, $"Добавить {host}?", "Добавление компьютера", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Settings.Default.Servers.Add(host);
                        Settings.Default.Save();
                        if ((sender as FrameworkElement)?.DataContext is MainViewModel mvm)
                            mvm.Servers.Add(new Server(host));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                adding = false;
            }

        }
    }
}