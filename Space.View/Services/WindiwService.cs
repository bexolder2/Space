using Space.Model.Enums;
using Space.View.Services.Interfaces;
using Space.ViewModel;
using System.Windows;

namespace Space.View.Services
{
    public class WindowService : IWindowService
    {
        private FightWindow FightWnd;
        private MarketWindow MarketWnd;

        public WindowService()
        {
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToFightWindow += NavigateToFight;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToMarketWindow += NavigateToMarket;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.CloseFightWindow += OnCloseFightWindow;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.DisableMainWindow += OnDisableMainWindow;
        }

        public void ShowWindow(WindowType type)
        {
            switch (type)
            {
                case WindowType.FightWindow:
                    FightWnd = new FightWindow();
                    FightWnd.ShowDialog();
                    break;
                case WindowType.MainWindow:
                    break;
                case WindowType.MarketWindow:
                    MarketWnd = new MarketWindow();
                    MarketWnd.Show();
                    break;
                case WindowType.UpgradeWindow:
                    break;
            }
        }

        public void CloseWindow(WindowType type)
        {
            switch (type)
            {
                case WindowType.FightWindow:
                    Application.Current.Dispatcher.Invoke(() => {
                        FightWnd.Close();
                    });
                    break;
                case WindowType.MainWindow:
                    break;
                case WindowType.MarketWindow:
                    break;
                case WindowType.UpgradeWindow:
                    break;
            }
        }

        public void LockWindow(WindowType type)
        {
            switch (type)
            {
                case WindowType.FightWindow:
                    break;
                case WindowType.MainWindow:
                    Application.Current.Dispatcher.Invoke(() => {
                        (App.Current.MainWindow as MainWindow).MainGrid.IsEnabled = false;
                    });
                    break;
                case WindowType.MarketWindow:
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

        public void CloseFightWnd()
        {
            CloseWindow(WindowType.FightWindow);
        }

        public void DisableMainWindow()
        {
            LockWindow(WindowType.MainWindow);
        }

        private void NavigateToFight(object sender, System.EventArgs e)
        {
            ShowFightWnd();
        }

        private void NavigateToMarket(object sender, System.EventArgs e)
        {
            ShowMarketWnd();
        }

        private void OnCloseFightWindow(object sender, System.EventArgs e)
        {
            CloseFightWnd();
        }

        private void OnDisableMainWindow(object sender, System.EventArgs e)
        {
            DisableMainWindow();
        }
    }
}
