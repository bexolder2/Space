﻿using GalaSoft.MvvmLight.Messaging;
using Space.Helpers;
using Space.Helpers.Interfaces;
using Space.Infrastructure.Helpers;
using Space.Infrastructure.Pirates;
using Space.Model.BindableBase;
using Space.Model.Constants;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System;
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
        private Cell selectedCell;
        private const int maxCellNumber = 1600;
        private const int cellDistance = 1000;
        private const int numberRowsOrColumns = 40;
        private Player player;
        private Point startCoordinates;

        public event EventHandler<EventArgs> NavigateToFightWindow;
        public event EventHandler<EventArgs> NavigateToMarketWindow;

        public MainViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            #region command initialization
            GiveUpCommand = new RelayCommand(OnGiveUpCommandExecuted, CanGiveUpCommandExecute);
            MoveCommand = new RelayCommand(OnMoveCommandExecuted, CanMoveCommandExecute);
            NewPositionClickCommand = new RelayCommand(OnNewPositionClickCommandExecuted, CanNewPositionClickCommandExecute);
            CollectCommand = new RelayCommand(OnCollectCommandExecuted, CanCollectCommandExecute);
            #endregion

            InitializeEmptyPoints();
            InitializeStation();
            InitializeMoons();
            InitializePlayer();
            InitializeTestData();

            Messenger.Default.Register<List<KeyValuePair<IBindableModel, Module>>>(this, UpdateShipModules);
            Messenger.Default.Register<bool>(this, CalculateShipParams);

            ShipConfigurator sc = new ShipConfigurator();
        }

        #region commands
        public ICommand GiveUpCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand NewPositionClickCommand { get; private set; }
        public ICommand CollectCommand { get; private set; }
        #endregion

        #region properties
        public Cell SelectedCell
        {
            get => selectedCell;
            set => Set(ref selectedCell, value);
        }

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
            Moon planet2;
            int moon1 = (int)planet1;
            if(moon1 >= 3)
            {
               planet2 = (Moon)2;
            }
            else
            {
                planet2 = (Moon)(moon1 + 1);
            }

            var pl1 = new Cell
            {
                CellType = CellType.Planet1,
                Name = planet1.ToString(),
                Coordinates = new CoordinateGenerator().Generate(ConvertCellsToPoints())
            };
            int index1 = (int)(pl1.Coordinates.X * numberRowsOrColumns + pl1.Coordinates.Y);
            Cells.RemoveAt(index1);
            Cells.Insert(index1, pl1);

            var pl2 = new Cell
            {
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
                        CellType = CellType.Empty,
                        Name = "",
                        Coordinates = new Point { X = j, Y = i }
                    });
                }
                    
            }
        }

        private void InitializePlayer()
        {
            Player = new Player();
            Player.Spaceship = new Spaceship();
            Player.Spaceship.CurrentCoordinates = new Point(-1, -1);
            Player.Resources = new ResourcesBase
            {
                CryptocurrencyValue = 2500,
                EnergyValue = 500000,
                OreValue = 0
            };

            #region test data
            Player.Spaceship.ShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new CommandCenter { BodyLimit = 4, HP = 10, Level = Level.First }, Module.CommandCenter));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Battery { HP = 10, Level = Level.Second }, Module.Battery));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Storage { HP = 10, Level = Level.Third }, Module.Storage));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Gun { HP = -5, Level = Level.First }, Module.Gun));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Collector { HP = 10, Level = Level.Second }, Module.Collector));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Converter { HP = -5, Level = Level.Third }, Module.Converter));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Engine { HP = -10, Level = Level.Third }, Module.Engine));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Engine { HP = -10, Level = Level.Third }, Module.Engine));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Repairer { HP = 10, Level = Level.First }, Module.Repairer));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Repairer { HP = 10, Level = Level.Third }, Module.Repairer));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.Third }, Module.EmptyBody));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new EmptyBody { Level = Level.Third }, Module.EmptyBody));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            //Player.Spaceship.ShipModules.Add(new KeyValuePair<IBindableModel, Module>(new Body { HP = 100, Level = Level.Second }, Module.Body));
            #endregion
        }

        private void InitializeTestData()
        {
            //Cells.RemoveAt(0);
            //Cells.Insert(0, new Cell
            //{
            //    CellType = CellType.Player,
            //    Name = "",
            //    Coordinates = new Point { X = 0, Y = 0 }
            //});
            Cells.RemoveAt(1);
            Cells.Insert(1, new Cell
            {
                Asteroid = new Asteroid
                {
                    Name = "%TY%NJK"
                },
                CellType = CellType.Asteroid,
                Name = "%TY%NJK",
                Coordinates = new Point { X = 1, Y = 0 }
            });
            Cells.RemoveAt(2);
            Cells.Insert(2, new Cell
            {
                CellType = CellType.Planet1,
                Name = Moon.Callisto.ToString(),
                Coordinates = new Point { X = 2, Y = 0 }
            });
            Cells.RemoveAt(3);
            Cells.Insert(3, new Cell
            {
                CellType = CellType.Planet2,
                Name = Moon.Io.ToString(),
                Coordinates = new Point { X = 3, Y = 0 }
            });
            Cells.RemoveAt(4);
            Cells.Insert(4, new Cell
            {
                CellType = CellType.Asteroid,
                Name = "",
                Coordinates = new Point { X = 4, Y = 0 }
            });
            Cells.RemoveAt(5);
            Cells.Insert(5, new Cell
            {
                CellType = CellType.Station,
                Name = "",
                Coordinates = new Point { X = 5, Y = 0 }
            });
        }
        #endregion

        private void CreateAsteroid()
        {
            Cell asteroid = new Cell
            {
                CellType = CellType.Asteroid,
                Coordinates = new CoordinateGenerator().Generate(ConvertCellsToPoints()),
                Asteroid = new Asteroid
                {
                    Name = new NameGenerator().Generate(),
                    NumberOfOre = new OreGenerator().Generate()
                }
            };
            asteroid.Asteroid.Coordinates = asteroid.Coordinates; 
            asteroid.Name = asteroid.Asteroid.Name;
        }

        public void UpdateShipModules(List<KeyValuePair<IBindableModel, Module>> newModules)
        {
            Player.Spaceship.ShipModules = newModules;

            var intersection = Player.Spaceship.ShipModules.Intersect(Constants.BaseComplectation).ToList();
            if (!intersection.Except(Constants.BaseComplectation).Any())
            {
                if (Player.Spaceship.CurrentCoordinates.X == -1 && Player.Spaceship.CurrentCoordinates.Y == -1)
                {
                    Player.Spaceship.CurrentCoordinates = new Point { X = 0, Y = 0 };
                    Cells.FirstOrDefault().CellType = IntersectionHelper.GetObjectsIntersection(Cells.FirstOrDefault());
                    var cell = Cells.FirstOrDefault();
                    Cells.Remove(cell);
                    Cells.Insert(0, cell);
                }
            }
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

        #region move
        private bool CanMoveCommandExecute(object _)
        {
            bool result = false;
            if (Cells.Count > 0 && SelectedCell != null)
            {
                result = true;
            }
            return result;
        }
        private void OnMoveCommandExecuted(object _)
        {
            if (SelectedCell != null)
            {
                startCoordinates = new Point(SelectedCell.Coordinates.X, SelectedCell.Coordinates.Y);
            }
        }

        private bool CanNewPositionClickCommandExecute(object _) => true;
        private void OnNewPositionClickCommandExecuted(object cell)
        {
            if (cell != null)
            {
                int startIndex = (int)(startCoordinates.Y * numberRowsOrColumns + startCoordinates.X);
                int finishIndex = Cells.IndexOf((Cell)cell);
                Point finishCoordinates = new Point(Cells[finishIndex].Coordinates.X, Cells[finishIndex].Coordinates.Y);
                if (finishIndex >= 0)
                {
                    if (startCoordinates.X >= 0 && startCoordinates.Y >= 0)
                    {
                        int distance = CalculateDistance(startCoordinates, finishCoordinates);
                        if (distance * cellDistance < Player.Spaceship.AvailablePowerReserve)
                        {
                            CellType newCellType = IntersectionHelper.GetObjectsIntersection(Cells[finishIndex]);
                            CellType oldCellType = IntersectionHelper.GetCellTypeWitoutPlayer(Cells[startIndex]);
                            Cell oldCell = Cells[startIndex];
                            oldCell.CellType = oldCellType;
                            Cell newCell = Cells[finishIndex];
                            newCell.CellType = newCellType;

                            Cells.RemoveAt(startIndex);
                            Cells.Insert(startIndex, oldCell);
                            Cells.RemoveAt(finishIndex);
                            Cells.Insert(finishIndex, newCell);

                            int consumptionPer100km = ShipPropertyCounter.GetEnergyСonsumption(ref player);
                            Player.Resources.EnergyValue -= consumptionPer100km * distance * 10;
                            CalculateShipParams(true);
                            startCoordinates = new Point(-1, -1);
                            Player.Spaceship.CurrentCoordinates = new Point(finishCoordinates.X, finishCoordinates.Y);
                        }
                    }
                }
            }
        }

        private int CalculateDistance(Point point1, Point point2)
        {
            return (int)((point2.X - point1.X) + 1 + (point2.Y - point1.Y));
        }
        #endregion

        #region collect
        private bool CanCollectCommandExecute(object parameter)
        {
            bool result = false;
            int cellIndex = (int)(Player.Spaceship.CurrentCoordinates.Y * numberRowsOrColumns + Player.Spaceship.CurrentCoordinates.X);

            if(parameter != null)
            {
                if (parameter.ToString() == "asteroid" && Cells[cellIndex].Asteroid != null)
                {
                    if (Cells[cellIndex].Asteroid.NumberOfOre > 0)
                    {
                        result = true;
                    }
                }
                else if (parameter.ToString() == "planet")
                {
                    var cell = Cells[cellIndex];
                    if (cell.CellType == CellType.Planet1 || cell.CellType == CellType.Planet2 ||
                        cell.CellType == CellType.PlayerAndPlanet1 || cell.CellType == CellType.PlayerAndPlanet2)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        private void OnCollectCommandExecuted(object parameter)
        {
            if (parameter.ToString() == "asteroid")
            {
                int asteroidIndex = (int)(Player.Spaceship.CurrentCoordinates.Y * numberRowsOrColumns + Player.Spaceship.CurrentCoordinates.X);
                if (Player.Resources.EnergyValue > 0)
                {
                    CollectOre(parameter.ToString(), asteroidIndex);
                }
            }
            else
            {
                if (Player.Resources.EnergyValue > 0)
                {
                    CollectOre(parameter.ToString());
                }
            }
            ShipPropertyCounter.CountAvailableDistance(ref player);

            bool isPiratesAttack = new PiratesProbabilityGenertor().Generate();
            if (isPiratesAttack)
            {
                OnNavigateToFightWindow(null);
                //todo: generate pirates ship => navigate to battle window
            }
        }

        private void CollectOre(string param, int index = 0)
        {
            int ValuePerIteration = GetNumberOfOrePerCollect();
            Player.Resources.EnergyValue -= 1;
            int oreAmount = ValuePerIteration;

            if (param == "asteroid")
            {
                int numberOfOre = Cells[index].Asteroid.NumberOfOre;
                if (numberOfOre < ValuePerIteration)
                {
                    oreAmount = numberOfOre;
                }
                Cells[index].Asteroid.NumberOfOre -= oreAmount;
            }
            Player.Resources.OreValue += oreAmount;
        }

        private int GetNumberOfOrePerCollect()
        {
            int result = 0;
            foreach(var module in Player.Spaceship.ShipModules)
            {
                if(module.Value == Module.Collector)
                {
                    result += ((Collector)module.Key).CollectPerCruise;
                }
            }
            return result;
        }

        public void OnNavigateToFightWindow(EventArgs args)
        {
            NavigateToFightWindow?.Invoke(this, args);
        }
        #endregion

        public void OnNavigateToMarketWindow(EventArgs args)
        {
            NavigateToMarketWindow?.Invoke(this, args);
        }

        private bool CanGiveUpCommandExecute(object _) => true;
        private void OnGiveUpCommandExecuted(object _)
        {


        }
    }
}
