using Space.Helpers.Interfaces;
using Space.Model.Enums;
using System.Collections.Generic;
using System.Windows;

namespace Space.Model.Modules
{
    public class Spaceship : BindableBase.BindableBase
    {
        private int hp;
        private int maximumHP;
        private int damage;
        private int availablePowerReserve;
        private List<KeyValuePair<IBindableModel, Module>> shipModules;
        private Point currentCoordinates;

        #region properties
        public int MaximumHP
        {
            get => maximumHP;
            set => Set(ref maximumHP, value);
        }

        public Point CurrentCoordinates
        {
            get => currentCoordinates;
            set => Set(ref currentCoordinates, value);
        }

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

        public List<KeyValuePair<IBindableModel, Module>> ShipModules
        {
            get => shipModules;
            set => Set(ref shipModules, value);
        }
        #endregion
    }
}
