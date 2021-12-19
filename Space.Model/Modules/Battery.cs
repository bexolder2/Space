using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Battery : BindableBase.BindableBase
    {
        private int price;
        private int xp;
        private Level level;
        private int limit;

        #region properties
        public int Price
        {
            get => price;
            set => Set(ref price, value);
        }

        public int XP
        {
            get => xp;
            set => Set(ref xp, value);
        }

        public Level Level
        {
            get => level;
            set => Set(ref level, value);
        }

        public int Limit
        {
            get => limit;
            set => Set(ref limit, value);
        }
        #endregion
    }
}
