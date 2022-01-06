using GalaSoft.MvvmLight.Messaging;
using Space.Helpers;
using Space.Helpers.Interfaces;
using Space.Infrastructure.Helpers;
using Space.Infrastructure.Pirates;
using Space.Model.BindableBase;
using Space.Model.Constants;
using Space.Model.Entities;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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
        private Pirate pirate;
        private int maxPlayerHP;
        private int maxPirateHP;
        private Point startCoordinates;
        private int winCounter = 0;

        public event EventHandler<EventArgs> NavigateToFightWindow;
        public event EventHandler<EventArgs> NavigateToMarketWindow;
        public event EventHandler<EventArgs> NavigateToConvertWindow;
        public event EventHandler<EventArgs> CloseFightWindow;
        public event EventHandler<EventArgs> DisableMainWindow;  

        private ShipConfigurator configurator;
        private BattlleLogger logger;
        private int battleNumber;
        private string logText;

        private MarketModel buyModel;
        private MarketModel sellModel;

        private ConvertModel convertModel;
        private Timer repairTimer;
        private bool isRepairProcessed = false;

        public MainViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            #region command initialization
            GiveUpCommand = new RelayCommand(OnGiveUpCommandExecuted, CanGiveUpCommandExecute);
            MoveCommand = new RelayCommand(OnMoveCommandExecuted, CanMoveCommandExecute);
            NewPositionClickCommand = new RelayCommand(OnNewPositionClickCommandExecuted, CanNewPositionClickCommandExecute);
            CollectCommand = new RelayCommand(OnCollectCommandExecuted, CanCollectCommandExecute);
            StartBattleCommand = new RelayCommand(OnStartBattleCommandExecuted, CanStartBattleCommandExecute);
            OpenMarketCommand = new RelayCommand(OnOpenMarketCommandExecuted, CanOpenMarketCommandExecute);
            BuyCommand = new RelayCommand(OnBuyCommandExecuted, CanBuyCommandExecute);
            SellCommand = new RelayCommand(OnSellCommandExecuted, CanSellCommandExecute);
            ConvertCommand = new RelayCommand(OnConvertCommandExecuted, CanConvertCommandExecute);
            RepairCommand = new RelayCommand(OnRepairCommandExecuted, CanRepairCommandExecute);
            OpenConvertCommand = new RelayCommand(OnOpenConvertCommandExecuted, CanOpenConvertCommandExecute);
            #endregion

            InitializeEmptyPoints();
            InitializeStation();
            InitializeMoons();
            InitializePlayer();
            InitializeTestData();

            Messenger.Default.Register<List<KeyValuePair<IBindableModel, Module>>>(this, UpdateShipModules);
            Messenger.Default.Register<bool>(this, CalculateShipParams);
            Messenger.Default.Register<string>(this, Win);

            configurator = new ShipConfigurator();
            logger = new BattlleLogger();
            battleNumber = 1;

            BuyModel = new MarketModel();
            SellModel = new MarketModel();
            ConvertModel = new ConvertModel();
        }

        #region commands
        public ICommand GiveUpCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand NewPositionClickCommand { get; private set; }
        public ICommand CollectCommand { get; private set; }
        public ICommand StartBattleCommand { get; private set; }
        public ICommand OpenMarketCommand { get; private set; }
        public ICommand BuyCommand { get; private set; }
        public ICommand SellCommand { get; private set; }
        public ICommand ConvertCommand { get; private set; }
        public ICommand RepairCommand { get; private set; }
        public ICommand OpenConvertCommand { get; private set; }
        #endregion

        #region properties
        public ConvertModel ConvertModel
        {
            get => convertModel;
            set => Set(ref convertModel, value);
        }

        public MarketModel BuyModel
        {
            get => buyModel;
            set => Set(ref buyModel, value);
        }

        public MarketModel SellModel
        {
            get => sellModel;
            set => Set(ref sellModel, value);
        }

        public string LogText
        {
            get => logText;
            set => Set(ref logText, value);
        }

        public int MaxPlayerHP
        {
            get => maxPlayerHP;
            set => Set(ref maxPlayerHP, value);
        }

        public int MaxPirateHP
        {
            get => maxPirateHP;
            set => Set(ref maxPirateHP, value);
        }

        public Pirate Pirate
        {
            get => pirate;
            set => Set(ref pirate, value);
        }

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
            CalculateShipsHpAndDamage();
            Player.Spaceship.HP = Player.Spaceship.MaximumHP;
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

        public void Win(string key)
        {
            OnDisableMainWindow(null);
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

        private int GetStorageCapacity()
        {
            int storageCapacity = 0;
            foreach (var item in Player.Spaceship.ShipModules)
            {
                if (item.Value == Module.Storage)
                {
                    storageCapacity += ((Storage)item.Key).Limit;
                }
            }
            return storageCapacity;
        }

        private int GetEnergyCapacity()
        {
            int energyCapacity = 0;
            foreach (var item in Player.Spaceship.ShipModules)
            {
                if (item.Value == Module.Battery)
                {
                    energyCapacity += ((Battery)item.Key).Limit;
                }
            }
            return energyCapacity;
        }

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
                Pirate = null;
                Pirate = configurator.CreatePirate();
                CalculateShipsHpAndDamage();
                OnNavigateToFightWindow(null);
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

                if (Cells[index].Asteroid.NumberOfOre == 0)
                {
                    Cells[index].CellType = CellType.Player;
                    Cells[index].Asteroid = null;
                    var tmp = Cells[index];
                    Cells.RemoveAt(index);
                    Cells.Insert(index, tmp);
                }
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
        #endregion

        #region battle
        public void OnNavigateToFightWindow(EventArgs args)
        {
            NavigateToFightWindow?.Invoke(this, args);
        }

        private void CalculateShipsHpAndDamage()
        {
            var spaceship = player.Spaceship;
            ShipPropertyCounter.CountDamageValue(ref spaceship);
            ShipPropertyCounter.CountHPValue(ref spaceship);
            MaxPlayerHP = spaceship.MaximumHP;

            if (pirate != null)
            {
                var piratesSpaceship = pirate.Spaceship;
                ShipPropertyCounter.CountDamageValue(ref piratesSpaceship);
                ShipPropertyCounter.CountHPValue(ref piratesSpaceship);
                MaxPirateHP = piratesSpaceship.MaximumHP;
                piratesSpaceship.HP = MaxPirateHP;
            }
        }
        
        private bool CanStartBattleCommandExecute(object _) => true;
        private void OnStartBattleCommandExecuted(object _)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += StartBattle;
            worker.RunWorkerAsync();
        }

        private void StartBattle(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            bool isPirateAttack = true;
            while (pirate.Spaceship.HP > 0 && player.Spaceship.HP > 0)
            {
                if (isPirateAttack)
                {
                    Player.Spaceship.HP -= Pirate.Spaceship.Damage;
                    LogText += logger.LogAction("Pirate", "Player", pirate.Spaceship.Damage, player.Spaceship.HP, battleNumber);
                }
                else
                {
                    Pirate.Spaceship.HP -= Player.Spaceship.Damage;
                    LogText += logger.LogAction("Player", "Pirate", player.Spaceship.Damage, pirate.Spaceship.HP, battleNumber);
                }
                isPirateAttack = !isPirateAttack;
                Thread.Sleep(1000);
            }
            battleNumber++;
            SetBattleResults();
        }

        private void SetBattleResults()
        {
            if (player.Spaceship.HP <= 0)
            {
                MessageBox.Show("You are lose :(\nTry again.");
                OnCloseFightWindow(null);
                OnDisableMainWindow(null);
            }
            else
            {
                MessageBox.Show("You are win :)");
                OnCloseFightWindow(null);

                int storageCapacity = GetStorageCapacity();

                if(storageCapacity - Player.Resources.OreValue >= 1000)
                {
                    Player.Resources.OreValue += 1000;
                }
                else
                {
                    Player.Resources.OreValue += storageCapacity - Player.Resources.OreValue;
                    MessageBox.Show("Склад заполнен.");
                }
                Player.Resources.EnergyValue += 100;

                winCounter++;

                if (winCounter == 3)
                {
                    winCounter = 0;
                    CreateAsteroid();
                }
            }
        }

        public void OnCloseFightWindow(EventArgs args)
        {
            CloseFightWindow?.Invoke(this, args);
        }

        public void OnDisableMainWindow(EventArgs args)
        {
            DisableMainWindow?.Invoke(this, args);
        }
        #endregion

        #region market
        private bool CanOpenMarketCommandExecute(object _) => true;
        private void OnOpenMarketCommandExecuted(object _)
        {
            OnNavigateToMarketWindow(null);
        }

        public void OnNavigateToMarketWindow(EventArgs args)
        {
            NavigateToMarketWindow?.Invoke(this, args);
        }

        private bool CanBuyCommandExecute(object _)
        {
            if(BuyModel.Value > 0)
            {
                if (BuyModel.Value > 0 && BuyModel.Value < 100)
                {
                    BuyModel.Price = BuyModel.Value * 0.5;
                }
                else if (BuyModel.Value >= 100 && BuyModel.Value < 500)
                {
                    BuyModel.Price = BuyModel.Value * 0.4;
                }
                else if (BuyModel.Value >= 500 && BuyModel.Value < 1500)
                {
                    if (BuyModel.IsDelivery)
                    {
                        BuyModel.Price = BuyModel.Value * 0.4;
                    }
                    else
                    {
                        BuyModel.Price = BuyModel.Value * 0.3;
                    }
                }
                else if (BuyModel.Value >= 1500)
                {
                    if (BuyModel.IsDelivery)
                    {
                        BuyModel.Price = BuyModel.Value * 0.3;
                    }
                    else
                    {
                        BuyModel.Price = BuyModel.Value * 0.1;
                    }
                }
            }
            return true;
        }
        private void OnBuyCommandExecuted(object _)
        {
            if (Player.Resources.CryptocurrencyValue - BuyModel.Price >= 0 && BuyModel.Value > 0)
            {
                if (BuyModel.IsDelivery && BuyModel.Value >= 500)
                {
                    BuyEnergy();
                }
                else
                {
                    if (ValidatePositionForMarketAction())
                    {
                        BuyEnergy();
                    }
                }
            }
        }

        private bool CanSellCommandExecute(object _)
        {
            if (SellModel.Value > 0)
            {
                if (SellModel.Value > 0 && SellModel.Value < 100)
                {
                    SellModel.Price = SellModel.Value * 0.12;
                }
                else if (SellModel.Value >= 100 && SellModel.Value < 500)
                {
                    SellModel.Price = SellModel.Value * 0.1;
                }
                else if (SellModel.Value >= 500 && SellModel.Value < 1500)
                {
                    SellModel.Price = SellModel.Value * 0.08;
                }
                else if (SellModel.Value >= 1500)
                {
                    SellModel.Price = SellModel.Value * 0.06;
                }
            }
            return true;
        }
        private void OnSellCommandExecuted(object _)
        {
            if (ValidatePositionForMarketAction())
            {
                if (Player.Resources.OreValue - SellModel.Value >= 0 && SellModel.Value > 0)
                {
                    Player.Resources.OreValue -= (int)SellModel.Value;
                    Player.Resources.CryptocurrencyValue += SellModel.Price;
                }
                else
                {
                    MessageBox.Show("Недостаточно руды!");
                }
            }
        }

        private bool ValidatePositionForMarketAction()
        {
            bool result = false;

            if (Cells.Contains(Cells.FirstOrDefault(x => x.CellType == CellType.PlayerAndStation)))
            {
                result = true;
            }
            else
            {
                MessageBox.Show("Нужно переместится на клетку со станцией чтобы торговать.");
            }

            return result;
        }

        private void BuyEnergy()
        {
            Player.Resources.CryptocurrencyValue -= BuyModel.Price;
            Player.Resources.EnergyValue += BuyModel.Value;
        }
        #endregion

        #region convert
        private bool CanOpenConvertCommandExecute(object _) => true;
        private void OnOpenConvertCommandExecuted(object _)
        {
            OnNavigateToConvertWindow(null);
        }

        private bool CanConvertCommandExecute(object _)
        {
            List<KeyValuePair<IBindableModel, Module>> converters = new List<KeyValuePair<IBindableModel, Module>>();
            foreach (var item in Player.Spaceship.ShipModules)
            {
                if(item.Value == Module.Converter)
                {
                    converters.Add(item);
                }
            }

            if (converters.Count > 0)
            {
                var converter = converters.Max(x => ((BaseModel)x.Key).Level);
                ConvertModel.OreValue = ConvertModel.EnergyValue * (5 - (uint)converter);
            }      

            return true;
        }
        private void OnConvertCommandExecuted(object _)
        {
            KeyValuePair<IBindableModel, Module> converter = new KeyValuePair<IBindableModel, Module>();
            foreach (var item in Player.Spaceship.ShipModules)
            {
                if (item.Value == Module.Converter)
                {
                    converter = item;
                }
            }

            if (Player.Spaceship.ShipModules.Where(x => x.Value == Module.Converter).Any())
            {
                if (ConvertModel.EnergyValue > 0)
                {
                    if (Player.Resources.EnergyValue + ConvertModel.EnergyValue < GetEnergyCapacity())
                    {
                        if (Player.Resources.OreValue > 0 && Player.Resources.OreValue - ConvertModel.OreValue > 0)
                        {
                            Player.Resources.EnergyValue += ConvertModel.EnergyValue;
                            Player.Resources.OreValue -= (int)ConvertModel.OreValue;
                        }
                        else
                        {
                            MessageBox.Show("Недостаточно руды.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно места для энергии, усовершенствуйте аккумулятор.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нужно купить конвертер чтобы конвертировать руду.");
            }
        }

        public void OnNavigateToConvertWindow(EventArgs args)
        {
            NavigateToConvertWindow?.Invoke(this, args);
        }
        #endregion

        #region repair
        private bool CanRepairCommandExecute(object _) => true;
        private void OnRepairCommandExecuted(object _)
        {
            CalculateShipsHpAndDamage();
            if (isRepairProcessed)
            {
                MessageBox.Show("Ремонт уже запущен.");
            }
            else
            {
                if (Player.Spaceship.HP < MaxPlayerHP)
                {
                    if (Player.Resources.EnergyValue - 10 > 0)
                    {
                        isRepairProcessed = true;
                        repairTimer = new Timer(new TimerCallback(RepairProgress), null, (int)TimeSpan.FromMinutes(10).TotalMilliseconds, Timeout.Infinite);
                        MessageBox.Show("Ремонт запущен он продлится 10 минут.");
                    }
                    else MessageBox.Show("Недостаточно денег для ремонта.");
                }
                else MessageBox.Show("Ремонт не нужен.");
            }
        }

        private void RepairProgress(object state)
        {
            isRepairProcessed = false;
            CalculateShipsHpAndDamage();
            Player.Spaceship.HP = MaxPlayerHP;
            MessageBox.Show("Ремонт окончен.");
        }
        #endregion

        private bool CanGiveUpCommandExecute(object _) => true;
        private void OnGiveUpCommandExecuted(object _)
        {
            MessageBox.Show("You are lose :(\nTry again.");
            OnDisableMainWindow(null);
        }
    }
}
