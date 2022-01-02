using Space.Model.Enums;
using System.Windows;

namespace Space.Model.Modules
{
    public class Cell : BindableBase.BindableBase
    {
        private CellType cellType;
        private string name;
        private Point coordinates;
        private Asteroid asteroid;

        #region properties
        public Asteroid Asteroid
        {
            get => asteroid;
            set => Set(ref asteroid, value);
        }

        public CellType CellType
        {
            get => cellType;
            set => Set(ref cellType, value);
        }

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public Point Coordinates
        {
            get => coordinates;
            set => Set(ref coordinates, value);
        }
        #endregion
    }
}
