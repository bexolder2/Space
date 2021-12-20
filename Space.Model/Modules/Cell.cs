using Space.Model.Enums;
using System.Windows;

namespace Space.Model.Modules
{
    public class Cell : BindableBase.BindableBase
    {
        private Action cellAction;
        private CellType cellType;
        private string name;
        private Point coordinates;

        #region properties
        public Action CellAction
        {
            get => cellAction;
            set => Set(ref cellAction, value);
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
