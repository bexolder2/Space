using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Space.View.CustomControls
{
    public partial class TopButtonsPanel : UserControl
    {
        public TopButtonsPanel()
        {
            InitializeComponent();
        }

        private void OpenUpgradeWnd(object sender, RoutedEventArgs e)
        {
            var upgradeWnd = new UpgradeWindow();
        }
    }
}
