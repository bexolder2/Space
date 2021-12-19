using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Efficiency : BindableBase.BindableBase
    {
        private int power;
        private int value;
        private ResourceType resourceType;

        #region properties
        public int Power
        {
            get => power;
            set => Set(ref power, value);
        }

        public int Value
        {
            get => value;
            set => Set(ref this.value, value);
        }

        public ResourceType ResourceType
        {
            get => resourceType;
            set => Set(ref resourceType, value);
        }
        #endregion
    }
}
