using Space.Model.Enums;
using Space.View.Services.Interfaces;
using Space.ViewModel;
using System.Windows;

namespace Space.View.Services
{
    public class WindowService : IWindowService
    {
        public WindowService()
        {
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToFightWindow += NavigateToFight;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToMarketWindow += NavigateToMarket;
        }

        public void ShowWindow(WindowType type)
        {
            switch (type)
            {
                case WindowType.FightWindow:
                    FightWindow FightWnd = new FightWindow();
                    FightWnd.ShowDialog();
                    break;
                case WindowType.MainWindow:
                    break;
                case WindowType.MarketWindow:
                    MarketWindow MarketWnd = new MarketWindow();
                    MarketWnd.Show();
                    break;
                case WindowType.UpgradeWindow:
                    break;
            }
        }

        public void ShowFightWnd()
        {
            ShowWindow(WindowType.FightWindow);
        }

        public void ShowMarketWnd()
        {
            ShowWindow(WindowType.MarketWindow);
        }

        private void NavigateToFight(object sender, System.EventArgs e)
        {
            ShowFightWnd();
        }

        private void NavigateToMarket(object sender, System.EventArgs e)
        {
            ShowMarketWnd();
        }
    }
}
