using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

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
            const string testMessage = "EchoMessageЭхоСообщение202";
            Servers = new ObservableCollection<Server>();
            foreach (var client in Properties.Settings.Default.Servers)
            {
                var req = WebRequest.CreateHttp($"http://{client}:8080/test/echo?mes={testMessage}");
                Runner.IgnoreErr(() =>
                {
                    var resp = req.GetResponse();
                    if (req.HaveResponse)
                    {
                        var str = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                        if (str.Equals(testMessage))
                        {
                            Servers.Add_s(new Server(client));
                        }
                    }
                });

            }
            _timer = new Timer(TimerWork, null, 5000, Properties.Settings.Default.RefreshPeriod);
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

        Timer _timer;
        private Server _selectedServer;

        private void TimerWork(object state)
        {
            Runner.InMainDispatcher(() => Servers.Refresh());
        }
    }
}
