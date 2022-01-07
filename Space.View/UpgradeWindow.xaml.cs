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
            (DataContext as UpgradeViewModel).InitializeShip();
        }

        //private void SetWindowState(WndAction action)
        //{
        //    switch (action)
        //    {
        //        case WndAction.Hide:
        //            Hide();
        //            (DataContext as UpgradeViewModel).IsFirstLaunch = false;
        //            break;
        //        case WndAction.Show:
        //            if (!(DataContext as UpgradeViewModel).IsFirstLaunch)
        //            {
        //                (DataContext as UpgradeViewModel).InitializeShip();
        //            }
        //            Show();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
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
            Close();
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            (DataContext as UpgradeViewModel).UpgradeCommand.Execute(null);
            ship.Items.Refresh();
        }

        private void LocateClick(object sender, RoutedEventArgs e)
        {
            (DataContext as UpgradeViewModel).LocateCommand();
            ship.Items.Refresh();
        }

        private void CancelLocateClick(object sender, RoutedEventArgs e)
        {
            (DataContext as UpgradeViewModel).CancelLocate();
            ship.Items.Refresh();
        }
    }
}
