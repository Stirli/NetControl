using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
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
using System.Windows.Threading;

namespace NetControlServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public ObservableCollection<Client> Clients
        {
            get => (ObservableCollection<Client>)GetValue(ClientsProperty);
            set => SetValue(ClientsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Clients.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClientsProperty =
            DependencyProperty.Register("Clients", typeof(ObservableCollection<Client>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<Client>()));


        public IPAddress RefreshState
        {
            get { return (IPAddress)GetValue(RefreshStateProperty); }
            set { SetValue(RefreshStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshStateProperty =
            DependencyProperty.Register("RefreshState", typeof(IPAddress), typeof(MainWindow), new PropertyMetadata(null));



        public bool Refreshing
        {
            get => (bool)GetValue(RefreshingProperty);
            set => SetValue(RefreshingProperty, value);
        }

        // Using a DependencyProperty as the backing store for Refreshing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RefreshingProperty =
            DependencyProperty.Register("Refreshing", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        private async void ScanButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            Refreshing = true;
            // доступно ли сетевое подключение
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return;
            Task.WaitAll();
            await NetScanner.ScanAllAsync(address =>
                             {
                                 WebRequest wr = WebRequest.CreateHttp("http://" + address + ":8080/test/echo?mes=message");
                                 WebResponse resp = null;
                                 try
                                 {
                                     resp = wr.GetResponse();
                                     var str = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                                     resp.Close();
                                     Dispatcher.Invoke(() => Clients.Add(str.Equals("message")
                                         ? new Client(address)
                                         : null));
                                 }
                                 catch (Exception ex)
                                 {
                                     // ignored
                                     MessageBox.Show(ex.Message);
                                 }
                             }, address =>
                Dispatcher.InvokeAsync(() => RefreshState = address));
            Refreshing = false;
        }

    }
}

