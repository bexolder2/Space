using System;
using System.Windows;

namespace Space.Model.Modules
{
    public class ResourcesBase : BindableBase.BindableBase
    {
        private double cryptocurrencyValue;
        private double energyValue;
        private int oreValue;
        public event EventHandler<double> EnergyValueChanged;

        #region properties
        public double CryptocurrencyValue
        {
            get => cryptocurrencyValue;
            set
            {
                Set(ref cryptocurrencyValue, value);
                if (value >= 5000)
                {
                    MessageBox.Show("You are win!");
                }
            }
        }

        public double EnergyValue
        {
            get => energyValue;
            set
            {
                Set(ref energyValue, value);
                EnergyValueChanged?.Invoke(this, value);
            }
        }

        public int OreValue
        {
            get => oreValue;
            set => Set(ref oreValue, value);
        }
        #endregion
    }
}
