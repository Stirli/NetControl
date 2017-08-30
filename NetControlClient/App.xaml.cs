using System;
using System.Reflection;
using System.Windows;
using NetControlCommon.Properties;
using NetControlCommon.Utils;

namespace NetControlClient
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            Runner.DefaultCatch = s =>
            {
                var e = 5;
                MessageBox.Show(s, Assembly.GetEntryAssembly().FullName);
            };
        }

        public static void InMainDispatcher([NotNull] Action action)
        {
            Current.Dispatcher.BeginInvoke(action);
        }
    }
}