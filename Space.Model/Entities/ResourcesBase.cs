namespace Space.Model.Modules
{
    public class ResourcesBase : BindableBase.BindableBase
    {
        private double cryptocurrencyValue;
        private double energyValue;
        private int oreValue;

        #region properties
        public double CryptocurrencyValue
        {
            get => cryptocurrencyValue;
            set => Set(ref cryptocurrencyValue, value);
        }

        public double EnergyValue
        {
            get => energyValue;
            set => Set(ref energyValue, value);
        }

        public int OreValue
        {
            get => oreValue;
            set => Set(ref oreValue, value);
        }
        #endregion
    }
}
