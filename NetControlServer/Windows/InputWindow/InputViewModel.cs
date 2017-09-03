using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NetControlServer.Annotations;

namespace NetControlServer.Windows.InputWindow
{
    public class InputViewModel : INotifyPropertyChanged
    {
        public InputViewModel()
        {
            var list = new InputGestureCollection { new KeyGesture(Key.Enter) };
            OkCommand = new RoutedUICommand("OK", "OkCommand", typeof(InputViewModel), list);
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        private object _input;
        private string _message;

        public object Input
        {
            get { return _input; }
            set
            {
                if (Equals(value, _input)) return;
                _input = value;
                OnPropertyChanged();
            }
        }

        public RoutedUICommand OkCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
