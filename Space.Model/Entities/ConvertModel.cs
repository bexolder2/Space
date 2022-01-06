namespace Space.Model.Entities
{
    public class ConvertModel : BindableBase.BindableBase
    {
        private uint energyValue;
        private uint oreValue;

        #region properties
        public uint EnergyValue
        {
            get => energyValue;
            set => Set(ref energyValue, value);
        }

        public uint OreValue
        {
            get => oreValue;
            set => Set(ref oreValue, value);
        }
        #endregion
    }
}
