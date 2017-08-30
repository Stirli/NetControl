using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NetControlCommon;
using NetControlCommon.Utils;
using NetControlServer.Properties;

namespace NetControlServer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        HttpServer server;
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Runner.DefaultCatch = s => MessageBox.Show(s);
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

