using Space.Model.Enums;
using Space.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Space.View
{
    public partial class UpgradeWindow : Window
    {
        public UpgradeWindow()
        {
            InitializeComponent();
            SetWindowState(WndAction.Show);
        }

        private void SetWindowState(WndAction action)
        {
            switch (action)
            {
                case WndAction.Hide:
                    Hide();
                    break;
                case WndAction.Show:
                    Show();
                    break;
                default:
                    break;
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as UpgradeViewModel).NewPositionClickCommand.Execute(ship.SelectedIndex);
            ship.Items.Refresh();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            SetWindowState(WndAction.Hide);
        }
    }
}
