using Space.Model.BindableBase;
using Space.Model.Enums;
using Space.Model.Modules;
using System.Collections.ObjectModel;

namespace Space.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<Cell> cells;

        public MainViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            //for (int i = 0; i < 40; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.Station});
            //}
            //for (int i = 0; i < 40; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.None });
            //}
            //for (int i = 0; i < 40; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.Planet1 });
            //}
            //for (int i = 0; i < 40; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.Planet2 });
            //}
            //for (int i = 0; i < 40; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.Asteroid });
            //}

            //for (int i = 0; i < 1400; i++)
            //{
            //    Cells.Add(new Cell { CellType = CellType.Planet2 });
            //}
        }

        #region properties
        public ObservableCollection<Cell> Cells
        {
            get => cells;
            set => Set(ref cells, value);
        }
        #endregion
    }
}
