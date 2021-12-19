using Space.Model.Enums;

namespace Space.Model.Modules
{
    public abstract class BaseModel : BindableBase.BindableBase
    {
        private int price;
        private int hp;
        private Level level;

        #region properties
        public int Price
        {
            get => price;
            set => Set(ref price, value);
        }

        public int HP
        {
            get => hp;
            set => Set(ref hp, value);
        }

        public Level Level
        {
            get => level;
            set => Set(ref level, value);
        }
        #endregion
    }
}
