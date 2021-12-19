using Space.Model.Enums;

namespace Space.Model.Modules
{
    public class Cell : BindableBase.BindableBase
    {
        private Action cellAction;
        private CellType cellType;

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
        #endregion
    }
}
