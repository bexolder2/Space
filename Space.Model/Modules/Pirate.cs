using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Pirate : Actor
    {
        private Level shipLevel;

        #region properties
        public Level ShipLevel
        {
            get => shipLevel;
            set => Set(ref shipLevel, value);
        }
        #endregion
    }
}
