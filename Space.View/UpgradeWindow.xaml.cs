using Space.Model.Enums;
using Space.ViewModel;
using System.Windows;

namespace Space.View
{
    public partial class UpgradeWindow : Window
    {
        public UpgradeWindow()
        {
            InitializeComponent();
            DataContext = new UpgradeViewModel();
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
    }
}
