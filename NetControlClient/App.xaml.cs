using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetControlCommon;
using NetControlCommon.Annotations;

namespace NetControlClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            Runner.DefaultCatch = s => MessageBox.Show(s);
        }

        public static void InMainDispatcher([NotNull] Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
