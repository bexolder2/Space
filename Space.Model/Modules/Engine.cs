using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Engine : BindableBase.BindableBase
    {
        private int price;
        private int xp;
        private Level level;
        private int energyConsymptionPerBattle;
        private int energyConsymptionPer100Kilometers;

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

        public int EnergyConsymptionPerBattle
        {
            get => energyConsymptionPerBattle;
            set => Set(ref energyConsymptionPerBattle, value);
        }

        public int EnergyConsymptionPer100Kilometers
        {
            get => energyConsymptionPer100Kilometers;
            set => Set(ref energyConsymptionPer100Kilometers, value);
        }
        #endregion
    }
}
