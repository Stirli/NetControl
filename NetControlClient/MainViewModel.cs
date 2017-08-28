using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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

        public ObservableCollection<Client> Clients { get; }

        public MainViewModel()
        {
            const string testMessage = "EchoMessageЭхоСообщение202";
            Clients = new ObservableCollection<Client>();
            foreach (var client in Properties.Settings.Default.Clients)
            {
                var req = WebRequest.CreateHttp($"http://{client}:8080/test/echo?mes={testMessage}");

                var resp = req.GetResponse();
                if (req.HaveResponse)
                {
                    var str = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    if (str.Equals(testMessage))
                    {
                        Clients.Add_s(new Client(client));
                    }
                }
            }
            _timer = new Timer(TimerWork,null,5000,5000);
        }

        Timer _timer;
        private void TimerWork(object state)
        {
            Clients.Refresh();
        }
    }
}
