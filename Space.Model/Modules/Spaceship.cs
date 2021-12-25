using Space.Helpers.Interfaces;
using Space.Model.Enums;
using System.Collections.Generic;

namespace Space.Model.Modules
{
    public class Spaceship : BindableBase.BindableBase
    {
        private int hp;
        private int damage;
        private int availablePowerReserve;
        private Dictionary<Dictionary<IBindableModel, Module>, int> shipModules;

        #region properties
        public int HP
        {
            get => hp;
            set => Set(ref hp, value);
        }

        public int Damage
        {
            get => damage;
            set => Set(ref damage, value);
        }

        public int AvailablePowerReserve
        {
            get => availablePowerReserve;
            set => Set(ref availablePowerReserve, value);
        }

        public Dictionary<Dictionary<IBindableModel, Module>, int> ShipModules
        {
            get => shipModules;
            set => Set(ref shipModules, value);
        }
        #endregion
    }
}
