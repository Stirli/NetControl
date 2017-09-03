using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Windows;
using NetControlCommon;
using NetControlCommon.Utils;
using NetControlServer.Properties;
using NetControlServer.Windows.InputWindow;

namespace NetControlServer
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private HttpServer server;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.HostName))
            {
                InputWindow inputw = new InputWindow("Введите имя хоста", Environment.MachineName);
                if (inputw.ShowDialog() == true)
                {
                    Settings.Default.HostName = inputw.Input.ToString();
                    Settings.Default.Save();
                    try
                    {
                        TcpClient client = new TcpClient(Settings.Default.HostName, 7878);
                        client.Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не запущен.");
                    App.Current.Shutdown();
                }
            }
            Runner.DefaultCatch = s => MessageBox.Show(s, Assembly.GetEntryAssembly().FullName);
            server = new HttpServer((st, cap) => MessageBox.Show(st, cap));
            server.Stopped += ServerStopped;
            server.StartAsync(Settings.Default.Prefixes.Cast<string>()).CatchWithMessageAsync();
            MessageBox.Show("Server started");
        }

        private void ServerStopped(object sender, EventArgs e)
        {
            MessageBox.Show("Server stoped");
            Shutdown();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
        }
    }
}