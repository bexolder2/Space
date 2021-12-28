using GalaSoft.MvvmLight.Messaging;
using Space.Helpers;
using Space.Helpers.Interfaces;
using Space.Infrastructure.Helpers;
using Space.Model.BindableBase;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Space.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private ObservableCollection<Cell> cells;
        private const int maxCellNumber = 1600;
        private const int numberRowsOrColumns = 40;
        private Player player;

        public MainViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            #region command initialization
            GiveUpCommand = new RelayCommand(OnGiveUpCommandExecuted, CanGiveUpCommandExecute);
            #endregion

            InitializeEmptyPoints();
            InitializeStation();
            InitializeMoons();
            InitializePlayer();
            InitializeTestData();

            Messenger.Default.Register<List<KeyValuePair<IBindableModel, Module>>>(this, UpdateShipModules);
            Messenger.Default.Register<bool>(this, CalculateShipParams);
        }

        #region commands
        public ICommand GiveUpCommand { get; private set; }
        #endregion

        #region properties
        public ObservableCollection<Cell> Cells
        {
            get => cells;
            set => Set(ref cells, value);
        }

        public Player Player
        {
            get => player;
            set => Set(ref player, value);
        }
        #endregion

        #region initialization
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

        private void InitializePlayer()
        {
            Player = new Player();
            Player.Spaceship = new Spaceship();
            Player.Resources = new ResourcesBase
            {
                CryptocurrencyValue = 2500,
                EnergyValue = 500000,
                OreValue = 0
            };

            #region test data
            Player.Spaceship.ShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new CommandCenter { BodyLimit = 4, HP = 10, Level = Level.First }, Module.CommandCenter));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Battery { HP = 10, Level = Level.Second }, Module.Battery));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Storage { HP = 10, Level = Level.Third }, Module.Storage));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Gun { HP = -5, Level = Level.First }, Module.Gun));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Collector { HP = 10, Level = Level.Second }, Module.Collector));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Converter { HP = -5, Level = Level.Third }, Module.Converter));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Engine { HP = -10, Level = Level.Third }, Module.Engine));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Engine { HP = -10, Level = Level.Third }, Module.Engine));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Repairer { HP = 10, Level = Level.First }, Module.Repairer));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Repairer { HP = 10, Level = Level.Third }, Module.Repairer));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.Third }, Module.EmptyBody));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.Third }, Module.EmptyBody));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            #endregion
        }

        private void InitializeTestData()
        {
            Cells.RemoveAt(0);
            Cells.Insert(0, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.Player,
                Name = "",
                Coordinates = new Point { X = 0, Y = 0 }
            });
            Cells.RemoveAt(1);
            Cells.Insert(1, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.Asteroid,
                Name = "",
                Coordinates = new Point { X = 1, Y = 0 }
            });
            Cells.RemoveAt(2);
            Cells.Insert(2, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.PlayerAndAsteroid,
                Name = "",
                Coordinates = new Point { X = 2, Y = 0 }
            });
            Cells.RemoveAt(3);
            Cells.Insert(3, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.PlayerAndPlanet1,
                Name = "",
                Coordinates = new Point { X = 3, Y = 0 }
            });
            Cells.RemoveAt(4);
            Cells.Insert(4, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.PlayerAndPlanet2,
                Name = "",
                Coordinates = new Point { X = 4, Y = 0 }
            });
            Cells.RemoveAt(5);
            Cells.Insert(5, new Cell
            {
                CellAction = Action.None,
                CellType = CellType.PlayerAndStation,
                Name = "",
                Coordinates = new Point { X = 5, Y = 0 }
            });
        }
        #endregion

        public void UpdateShipModules(List<KeyValuePair<IBindableModel, Module>> newModules)
        {
            Player.Spaceship.ShipModules = newModules;
        }

        public void CalculateShipParams(bool flag)
        {
            if (flag)
            {
                ShipPropertyCounter.CountAvailableDistance(ref player);
                var spaceship = player.Spaceship;
                ShipPropertyCounter.CountDamageValue(ref spaceship);
                ShipPropertyCounter.CountHPValue(ref spaceship);
            }
        }

        private List<Point> ConvertCellsToPoints()
        {
            List<Point> result = new List<Point>();
            result = Cells.Where(cell => cell.CellType != CellType.Empty)
                          .Join(Cells, x => x.Coordinates, y => y.Coordinates, 
                                (x, y) => new Point { X = x.Coordinates.X, Y = y.Coordinates.Y })
                          .ToList();

            return result;
        }

        private bool CanGiveUpCommandExecute(object p) => true;
        private void OnGiveUpCommandExecuted(object p)
        {


        }
    }
}
