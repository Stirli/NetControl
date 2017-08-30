using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using NetControlClient.Properties;
using NetControlCommon;

namespace NetControlClient
{
    class MainViewModel : ViewModel
    {
        private bool _refreshing;
        private IPAddress _refreshState;

        public bool Refreshing
        {
            get => _refreshing;
            private set
            {
                if (value == _refreshing) return;
                _refreshing = value;
                OnPropertyChanged();
            }
        }

        public IPAddress RefreshState
        {
            get => _refreshState;
            set
            {
                if (Equals(value, _refreshState)) return;
                _refreshState = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Server> Servers { get; }

        public MainViewModel()
        {
            Servers = new ObservableCollection<Server>();
            foreach (var client in Settings.Default.Servers)
            {
                Servers.Add_s(new Server(client));
            }
            task = TimerWork().ContinueWith(ContinueTask);
        }

        private void ContinueTask(Task task1)
        {
            task = TimerWork().ContinueWith(ContinueTask);
        }
        public Server SelectedServer
        {
            get => _selectedServer;
            set
            {
                if (Equals(value, _selectedServer)) return;
                _selectedServer = value;
                OnPropertyChanged();
            }
        }

        private Task task;
        private Server _selectedServer;

        private async Task TimerWork()
        {
            await Servers.RefreshAsync().CatchWithMessageAsync();
            Thread.Sleep(Settings.Default.RefreshPeriod);
        }
    }
}
