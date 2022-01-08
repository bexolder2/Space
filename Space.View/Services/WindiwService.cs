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
        private ConvertWindow ConvertWnd;
        private UpgradeWindow UpgradeWnd;

        public WindowService()
        {
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToFightWindow += NavigateToFight;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToMarketWindow += NavigateToMarket;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToConvertWindow += NavigateToConvert;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.CloseFightWindow += OnCloseFightWindow;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.CloseUpgradeWindow += OnCloseUpgradeWindow;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.DisableMainWindow += OnDisableMainWindow;
            (Application.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.NavigateToUpgradeWindow += NavigateToUpgrade;
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
                    MarketWnd.ShowDialog();
                    break;
                case WindowType.UpgradeWindow:
                    UpgradeWnd = new UpgradeWindow();
                    UpgradeWnd.ShowDialog();
                    break;
                case WindowType.ConvertWindow:
                    ConvertWnd = new ConvertWindow();
                    ConvertWnd.ShowDialog();
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
                    Application.Current.Dispatcher.Invoke(() => {
                        UpgradeWnd.Close();
                    });
                    break;
                case WindowType.ConvertWindow:
                    Application.Current.Dispatcher.Invoke(() => {
                        ConvertWnd.Close();
                    });
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
                case WindowType.ConvertWindow:
                    break;
            }
        }

        #region navigate
        public void ShowFightWnd()
        {
            ShowWindow(WindowType.FightWindow);
        }

        public void ShowMarketWnd()
        {
            ShowWindow(WindowType.MarketWindow);
        }

        public void ShowConvertWnd()
        {
            ShowWindow(WindowType.ConvertWindow);
        }

        public void ShowUpgradeWnd()
        {
            ShowWindow(WindowType.UpgradeWindow);
        }

        private void NavigateToFight(object sender, System.EventArgs e)
        {
            ShowFightWnd();
        }

        private void NavigateToMarket(object sender, System.EventArgs e)
        {
            ShowMarketWnd();
        }

        private void NavigateToConvert(object sender, System.EventArgs e)
        {
            ShowConvertWnd();
        }

        private void NavigateToUpgrade(object sender, System.EventArgs e)
        {
            ShowUpgradeWnd();
        }
        #endregion

        #region close
        private void OnCloseFightWindow(object sender, System.EventArgs e)
        {
            CloseFightWnd();
        }

        private void OnCloseConvertWindow(object sender, System.EventArgs e)
        {
            CloseConvertWnd();
        }

        private void OnCloseUpgradeWindow(object sender, System.EventArgs e)
        {
            CloseUpgradeWnd();
        }

        public void CloseUpgradeWnd()
        {
            CloseWindow(WindowType.UpgradeWindow);
        }

        public void CloseFightWnd()
        {
            CloseWindow(WindowType.FightWindow);
        }

        public void CloseConvertWnd()
        {
            CloseWindow(WindowType.ConvertWindow);
        }
        #endregion

        public void DisableMainWindow()
        {
            LockWindow(WindowType.MainWindow);
        }

        private void OnDisableMainWindow(object sender, System.EventArgs e)
        {
            DisableMainWindow();
        }  
    }
}
