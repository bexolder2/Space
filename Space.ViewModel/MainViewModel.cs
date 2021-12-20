using Space.Helpers;
using Space.Model.BindableBase;
using Space.Model.Enums;
using Space.Model.Modules;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Space.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<Cell> cells;
        private const int maxCellNumber = 1600;
        private const int numberRowsOrColumns = 40;

        public MainViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            InitializeEmptyPoints();
            InitializeStation();
            InitializeMoons();
        }

        #region properties
        public ObservableCollection<Cell> Cells
        {
            get => cells;
            set => Set(ref cells, value);
        }
        #endregion

        private void InitializeMoons()
        {
            Moon planet1 = (Moon)new MoonGenerator().Generate();
            Moon planet2 = (Moon)new MoonGenerator().Generate();

            var pl1 = new Cell
            {
                CellAction = Action.None,
                CellType = CellType.Planet1,
                Name = planet1.ToString(),
                Coordinates = new CoordinateGenerator().Generate(ConvertCellsToPoints())
            };
            int index1 = (int)(pl1.Coordinates.X * numberRowsOrColumns + pl1.Coordinates.Y);
            Cells.RemoveAt(index1);
            Cells.Insert(index1, pl1);

            var pl2 = new Cell
            {
                CellAction = Action.None,
                CellType = CellType.Planet2,
                Name = planet2.ToString(),
                Coordinates = new CoordinateGenerator().Generate(ConvertCellsToPoints())
            };
            int index2 = (int)(pl2.Coordinates.X * numberRowsOrColumns + pl2.Coordinates.Y);
            Cells.RemoveAt(index2);
            Cells.Insert(index2, pl2);
        }

        private void InitializeStation()
        {
            Cells.RemoveAt(820);
            Cells.Insert(820, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.Station,
                Name = "",
                Coordinates = new Point { X = 20, Y = 20 }
            });
        }

        private void InitializeEmptyPoints()
        {
            for(int i = 0; i < numberRowsOrColumns; i++)
            {
                for (int j = 0; j < numberRowsOrColumns; j++)
                {
                    Cells.Add(new Cell
                    {
                        CellAction = Action.None,
                        CellType = CellType.Empty,
                        Name = "",
                        Coordinates = new Point { X = i, Y = j }
                    });
                }
                    
            }
        }

        private List<Point> ConvertCellsToPoints()
        {
            List<Point> result = new List<Point>();
            result = Cells.Where(cell => cell.CellType != CellType.Empty).Join(Cells, x => x.Coordinates, y => y.Coordinates, 
                                (x, y) => new Point { X = x.Coordinates.X, Y = y.Coordinates.Y })
                                .ToList();

            return result;
        }
    }
}
