using Space.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Space.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.WindowService = new Services.WindowService();
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MainViewModel).NewPositionClickCommand.Execute(((ListViewItem)sender).Content);
        }
    }
}
