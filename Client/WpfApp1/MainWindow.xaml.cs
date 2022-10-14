using System.Windows;
using System.Windows.Input;

namespace WpfApp
{
    public sealed partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm = new();

        public MainWindow()
        {
            DataContext = _vm;
            InitializeComponent();
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await _vm.Send();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => _vm.Init();
    }
}