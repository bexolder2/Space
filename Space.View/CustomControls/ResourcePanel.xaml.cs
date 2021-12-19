using System.Windows;
using System.Windows.Controls;

namespace Space.View.CustomControls
{
    public partial class ResourcePanel : UserControl
    {
        public ResourcePanel()
        {
            InitializeComponent();
        }

        #region Dependency properties
        public static readonly DependencyProperty CryptocurrencyProperty =
            DependencyProperty.Register("Cryptocurrency",
                typeof(double),
                typeof(ResourcePanel));

        public double Cryptocurrency
        {
            get => (double)GetValue(CryptocurrencyProperty);
            set => SetValue(CryptocurrencyProperty, value);
        }

        public static readonly DependencyProperty EnergyProperty =
            DependencyProperty.Register("Energy",
                typeof(int),
                typeof(ResourcePanel));

        public int Energy
        {
            get => (int)GetValue(EnergyProperty);
            set => SetValue(EnergyProperty, value);
        }

        public static readonly DependencyProperty OreProperty =
            DependencyProperty.Register("Ore",
                typeof(int),
                typeof(ResourcePanel));

        public int Ore
        {
            get => (int)GetValue(OreProperty);
            set => SetValue(OreProperty, value);
        }
        #endregion
    }
}
