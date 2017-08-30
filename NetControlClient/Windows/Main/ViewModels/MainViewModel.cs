using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NetControlClient.MVVM;
using NetControlClient.Properties;
using NetControlClient.Utils;
using NetControlCommon;
using NetControlCommon.Utils;

namespace NetControlClient.Windows.Main.ViewModels
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
            RunRefreshTask();
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

        private Server _selectedServer;

        private Task task;
        private void RunRefreshTask(Task task1=null)
        {
            task = DoRefreshTask().ContinueWith(RunRefreshTask);
        }
        private async Task DoRefreshTask()
        {
            await Servers.RefreshAsync().CatchWithMessageAsync();
            Thread.Sleep(Settings.Default.RefreshPeriod);
        }
    }
}
