using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Gun : BindableBase.BindableBase
    {
        private int price;
        private int xp;
        private Level level;
        private int damage;

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

        public int Damage
        {
            get => damage;
            set => Set(ref damage, value);
        }
        #endregion
    }
}
