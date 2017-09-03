using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetControlServer.Windows.InputWindow
{
    /// <summary>
    /// Логика взаимодействия для InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow(string message, object defaultInput)
        {
            InitializeComponent();
            var inputViewModel = new InputViewModel { Message = message, Input = Input = defaultInput };
            DataContext = inputViewModel;
            CommandBindings.Add(new CommandBinding(inputViewModel.OkCommand, (sender, args) =>
            {
                Input = inputViewModel.Input;
                DialogResult = true;
                Close();
            }));
        }

        public object Input { get; private set; }
    }
}
