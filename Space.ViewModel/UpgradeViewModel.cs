using GalaSoft.MvvmLight.Messaging;
using Space.Helpers.Interfaces;
using Space.Infrastructure.Converters;
using Space.Infrastructure.Deserializer;
using Space.Model.BindableBase;
using Space.Model.Constants;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Space.ViewModel
{
    public class UpgradeViewModel : BindableBase
    {
        private Dictionary<IBindableModel, Module> modules;
        private KeyValuePair<IBindableModel, Module> selectedModule;
        private Dictionary<IBindableModel, Module> selectedModules;
        private KeyValuePair<IBindableModel, Module> selectedLevel;
        private string additionalSelectedLevelData;

        private List<KeyValuePair<IBindableModel, Module>> playersShipModules;
        private List<KeyValuePair<IBindableModel, Module>> buyBuffer;
        private List<KeyValuePair<IBindableModel, Module>> bodies;
        private int selectedModuleIndex;
        private int startIndex, finishIndex;
        private EmptyBody lastEmptyBody;
        private IBindableModelToModelTextConverter modelConverter = new IBindableModelToModelTextConverter();

        public UpgradeViewModel()
        {
            #region command initialization
            UpgradeCommand = new RelayCommand(OnUpgradeCommandExecuted, CanUpgradeCommandExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            MoveCommand = new RelayCommand(OnMoveCommandExecuted, CanMoveCommandExecute);
            NewPositionClickCommand = new RelayCommand(OnNewPositionClickCommandExecuted, CanNewPositionClickCommandExecute);
            BuyCommand = new RelayCommand(OnBuyCommandExecuted, CanBuyCommandExecute);
            #endregion

            modules = new Dictionary<IBindableModel, Module>();
            PlayersShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            IsFirstLaunch = true;
            buyBuffer = new List<KeyValuePair<IBindableModel, Module>>();

            Initialize();
            InitializeSelectedModules();
            InitializeShip();
        }

        #region commands
        public ICommand UpgradeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand NewPositionClickCommand { get; private set; }
        public ICommand BuyCommand { get; private set; }
        #endregion

        #region properties
        public bool IsFirstLaunch { get; set; }

        public List<KeyValuePair<IBindableModel, Module>> Bodies
        {
            get => bodies;
            set => Set(ref bodies, value);
        }

        public int SelectedModuleIndex
        {
            get => selectedModuleIndex;
            set => Set(ref selectedModuleIndex, value);
        }

        public List<KeyValuePair<IBindableModel, Module>> PlayersShipModules
        {
            get => playersShipModules;
            set => Set(ref playersShipModules, value);
        }

        public Dictionary<IBindableModel, Module> Modules
        {
            get => modules;
            set => Set(ref modules, value);
        }

        public KeyValuePair<IBindableModel, Module> SelectedModule
        {
            get => selectedModule;
            set
            {
                Set(ref selectedModule, value);
                SelectedModules = Modules.Where(item => item.Value == value.Value)
                                         .ToDictionary(_key => _key.Key, _value => _value.Value);
            }
        }

        public Dictionary<IBindableModel, Module> SelectedModules
        {
            get => selectedModules;
            set => Set(ref selectedModules, value);
        }

        public KeyValuePair<IBindableModel, Module> SelectedLevel
        {
            get => selectedLevel;
            set
            {
                Set(ref selectedLevel, value);
                AdditionalSelectedLevelData = modelConverter.Convert(value, null, null, null).ToString();
            }
        }

        public string AdditionalSelectedLevelData
        {
            get => additionalSelectedLevelData;
            set => Set(ref additionalSelectedLevelData, value);
        } 
        #endregion

        #region initializers
        private void Initialize()
        {
            var result = JsonDeserializer.InitializeModules();
            FillModules(result.Battery, Module.Battery);
            FillModules(result.Body, Module.Body);
            FillModules(result.Collector, Module.Collector);
            FillModules(result.CommandCenter, Module.CommandCenter);
            FillModules(result.Converter, Module.Converter);
            FillModules(result.Engine, Module.Engine);
            FillModules(result.Generator, Module.Generator);
            FillModules(result.Gun, Module.Gun);
            FillModules(result.Repairer, Module.Repairer);
            FillModules(result.Storage, Module.Storage);
        }

        private void FillModules<T>(List<T> module, Module moduleType) where T : IBindableModel 
        {
            foreach(T item in module)
            {
                modules.Add(item, moduleType);
            }
        }

        private void InitializeSelectedModules()
        {
            SelectedModule = new KeyValuePair<IBindableModel, Module>();
            SelectedModule = Modules.FirstOrDefault();
            SelectedModules = new Dictionary<IBindableModel, Module>();

            SelectedModules = Modules.Where(item => item.Value == SelectedModule.Value)
                                     .ToDictionary(_key => _key.Key, _value => _value.Value);
        }

        public void InitializeShip()
        {
            PlayersShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            PlayersShipModules = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Spaceship?.ShipModules;
            Bodies = new List<KeyValuePair<IBindableModel, Module>>();

            foreach(var item in PlayersShipModules)
            {
                if(item.Value is Module.Body)
                {
                    Bodies.Add(item);
                }
            }

            for (int i = 0; i < Bodies.Count; i++)
            {
                PlayersShipModules.Remove(Bodies[i]);
            }
        }
        #endregion

        #region validation
        private bool ValidateMinimalConfiguration()
        {
            bool result = false;

            Dictionary<Module, int> shipModules = GetShipModules();
            int counter = 0;
            foreach(var module in shipModules)
            {
                if (module.Value >= Constants.MinimalShipConfiguration?.Where(x => x.Key == module.Key).FirstOrDefault().Value)
                {
                    counter++;
                }
            }

            if (counter >= 8)
                result = true;

            return result;
        }

        private Dictionary<Module, int> GetShipModules()
        {
            Dictionary<Module, int> shipModules = new Dictionary<Module, int>();

            foreach (var module in Enum.GetValues(typeof(Module)))
            {
                int numberOfModules = PlayersShipModules.Where(x => x.Value == (Module)module).ToList().Count;
                shipModules.Add((Module)module, numberOfModules);
            }

            return shipModules;
        }

        #region validation number of modules
        private bool ValidateNumberOfModules()
        {
            bool result = false;

            if (ValidateNumberOfCommandCenters() && ValidateNumberOfEngines())
                result = true;

            return result;
        }

        private bool ValidateNumberOfCommandCenters()
        {
            bool result = false;
            Dictionary<Module, int> shipModules = GetShipModules();

            if (shipModules.FirstOrDefault(x => x.Key == Module.CommandCenter).Value == 1)
                result = true;

            return result;
        }

        private bool ValidateNumberOfEngines()
        {
            bool result = false;
            Dictionary<Module, int> shipModules = GetShipModules();
            int bodies = Bodies.Count;
            int engines = shipModules.FirstOrDefault(x => x.Key == Module.Engine).Value;

            if ((bodies / 2 + bodies % 2) >= engines)
                result = true;

            return result;
        }

        private bool ValidateNumberOfBodies()
        {
            bool result = false;
            Dictionary<Module, int> shipModules = GetShipModules();
            int bodies = Bodies.Count;

            if (bodies * 4 >= CalculateTotalNumbersOfModules() && 
                ((CommandCenter)PlayersShipModules
                .FirstOrDefault(x => x.Value == Module.CommandCenter).Key)
                .BodyLimit >= bodies)
                result = true;

            return result;
        }

        private int CalculateTotalNumbersOfModules()
        {
            int result = 0;
            Dictionary<Module, int> shipModules = GetShipModules();

            foreach(var module in shipModules)
            {
                if (module.Key != Module.Body)
                    result += module.Value;
            }

            return result;
        }
        #endregion

        private bool ValidationByPrice()
        {
            bool result = false;

            double budget = (double)(Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Resources?.CryptocurrencyValue;
            if (budget - ((BaseModel)(SelectedLevel.Key)).Price >= 0)
                result = true;

            return result;
        }

        #region validation location
        private bool CheckModulesCompatibility(Module module1, Module module2)
        {
            return Constants.ModulesLocationValidator[(int)module1][(int)module2];
        }

        private bool ValidateLocation(int index)
        {
            bool result = true;
            List<bool> compatibilities = new List<bool>();

            if (PlayersShipModules.Count > index)
            {
                if (index == 0)
                {
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationPlusFour(index));
                }
                else if (index == PlayersShipModules.Count - 1)
                {
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationMinusFour(index));
                }
                else if (index == 3)
                {
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationPlusFour(index));
                }
                else if (index == PlayersShipModules.Count - Constants.NumberOfModulesInOneBody)
                {
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationMinusFour(index));
                }
                else if (index % 4 == 0) //left line
                {
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationMinusFour(index));
                    compatibilities.Add(LocationPlusFour(index));
                }
                else if (index < Constants.NumberOfModulesInOneBody) //up
                {
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationPlusFour(index));
                }
                else if (index % Constants.NumberOfModulesInOneBody == 3) //right
                {
                    compatibilities.Add(LocationMinusFour(index));
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationPlusFour(index));
                }
                else if (index > PlayersShipModules.Count - Constants.NumberOfModulesInOneBody) //down
                {
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationMinusFour(index));
                }
                else
                {
                    compatibilities.Add(LocationPlusOne(index));
                    compatibilities.Add(LocationMinusOne(index));
                    compatibilities.Add(LocationPlusFour(index));
                    compatibilities.Add(LocationMinusFour(index));
                }
            }

            foreach(var item in compatibilities)
            {
                result = result && item;
            }

            return result;
        }

        private bool LocationMinusOne(int index)
        {
            return CheckModulesCompatibility(PlayersShipModules[index].Value,
                                             PlayersShipModules[index - 1].Value);
        }

        private bool LocationPlusOne(int index)
        {
            return CheckModulesCompatibility(PlayersShipModules[index].Value,
                                             PlayersShipModules[index + 1].Value);
        }

        private bool LocationMinusFour(int index)
        {
            return CheckModulesCompatibility(PlayersShipModules[index].Value,
                                             PlayersShipModules[index - Constants.NumberOfModulesInOneBody].Value);
        }

        private bool LocationPlusFour(int index)
        {
            return CheckModulesCompatibility(PlayersShipModules[index].Value,
                                             PlayersShipModules[index + Constants.NumberOfModulesInOneBody].Value);
        }
        #endregion

        private bool ValidateHP()
        {
            bool result = false;

            int hp = 0;
            foreach(var module in PlayersShipModules)
            {
                hp += ((BaseModel)module.Key).HP;
            }
            foreach(var body in Bodies)
            {
                hp += ((Body)body.Key).HP;
            }
            
            if (hp > 0)
                result = true;

            return result;
        }

        private bool Validate()
        {
            bool result = false;
            System.Diagnostics.Debug.WriteLine("Validate");
            if (ValidateNumberOfCommandCenters())
            {
                if (ValidateNumberOfModules())
                {
                    if (ValidateHP())
                    {
                        if (ValidationByPrice())
                        {
                            if (ValidateNumberOfBodies())
                            {
                                result = true;
                            }
                            else MessageBox.Show("Слишком много модулей");
                        }
                        else MessageBox.Show("Недостаточно средств для покупки модуля");
                    }
                    else MessageBox.Show("Отрицательная сумма брони");
                }
                else MessageBox.Show("Некорректное колличество командных центров/двигателей");
            }
            else MessageBox.Show("На корабле может быть только один командный центр");

            return result;
        }
        #endregion

        private bool CanUpgradeCommandExecute(object p)
        {
            return true;
        }
        private void OnUpgradeCommandExecuted(object selectedItem) //TODO: add buy logic and validation
        { 
            if (selectedItem != null)
            {
                var newModule = (KeyValuePair<IBindableModel, Module>)selectedItem;

                if (ValidateNumberOfCommandCenters())
                {
                    if (ValidateNumberOfModules())
                    {
                        if (ValidateNumberOfEngines())
                        {
                            if (ValidateHP())
                            {
                                if (ValidationByPrice())
                                {
                                    if (ValidateLocation(0))
                                    {
                                        if (newModule.Value is Module.Body)
                                        {
                                            Bodies.Add(new KeyValuePair<IBindableModel, Module>(new Body
                                            {
                                                HP = ((Body)newModule.Key).HP,
                                                Level = ((Body)newModule.Key).Level
                                            }, Module.Body));

                                            for (int i = 0; i < Constants.NumberOfModulesInOneBody; i++)
                                            {
                                                PlayersShipModules.Add(new KeyValuePair<IBindableModel, Module>(
                                                    new EmptyBody
                                                    {
                                                        HP = ((Body)newModule.Key).HP,
                                                        Level = ((Body)newModule.Key).Level
                                                    }, Module.EmptyBody));
                                            }
                                        }
                                        else
                                        {
                                            if (ValidateNumberOfBodies())
                                            {

                                            }
                                            else MessageBox.Show("Слишком много модулей данного типа");
                                        }
                                    }
                                    else MessageBox.Show("Недопустимое расположение");
                                }
                                else MessageBox.Show("Недостаточно средств для покупки модуля");
                            }
                            else MessageBox.Show("Отрицательное сумма брони");
                        }
                        else MessageBox.Show("Некорректное колличество двигателей");
                    }
                    else MessageBox.Show("Слишком много модулей, нужен еще один корпус");
                }
                else MessageBox.Show("На корабле может быть только один командный центр");
            }
        }

        private bool CanCancelCommandExecute(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {
            foreach(var item in Bodies)
            {
                PlayersShipModules.Add(item);
            }
            var result = PlayersShipModules.Except(buyBuffer).ToList();

            Messenger.Default.Send(result);
        }

        private bool CanMoveCommandExecute(object p)
        {
            if (PlayersShipModules.Count > 0)
                return true;
            else return false;
        }
        private void OnMoveCommandExecuted(object p)
        {
            startIndex = selectedModuleIndex;
        }

        #region modules moving
        private bool CanNewPositionClickCommandExecute(object p) => true;
        private void OnNewPositionClickCommandExecuted(object index)
        {
            if(index != null && index is int)
            {
                finishIndex = (int)index;

                if(startIndex >= 0 && finishIndex >= 0)
                {
                    SwapItems(startIndex, finishIndex);
                    startIndex = finishIndex = -1;
                }
            }
        }

        private void SwapItems(int index1, int index2)
        {
            var firstItem = PlayersShipModules[index1];
            PlayersShipModules[index1] = PlayersShipModules[index2];
            PlayersShipModules[index2] = firstItem;
        }
        #endregion

        #region buy
        private bool CanBuyCommandExecute(object p) => true;
        private void OnBuyCommandExecuted(object p)
        {
            var player = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player;

            foreach (var module in buyBuffer)
            {
                if (Validate())
                {
                    if(module.Value == Module.Body)
                    {
                        Buy(player, module);
                    }
                    else
                    {
                        if (ValidateLocation(PlayersShipModules.IndexOf(module)))
                        {
                            Buy(player, module);
                        }
                        else MessageBox.Show("Недопустимое расположение");
                    }                  
                }
            }
        }

        private void Buy(Player player, KeyValuePair<IBindableModel, Module> module)
        {
            if (((BaseModel)module.Key).Level == Level.First)
            {
                player.Resources.CryptocurrencyValue -= ((BaseModel)module.Key).Price;
                Bodies.Clear();
            }
            else
            {
                var currentlySelectedModule = Modules.Where(item => item.Value == module.Value)
                                                     .ToDictionary(_key => _key.Key, _value => _value.Value);
                player.Resources.CryptocurrencyValue -= ((BaseModel)currentlySelectedModule.FirstOrDefault().Key).Price;
                Bodies.Clear();
            }
            MessageBox.Show($"Модуль {module.Value} куплен");
        }
        #endregion

        public void LocateCommand()
        {
            if (selectedLevel.Value is Module.Body)
            {
                Bodies.Add(new KeyValuePair<IBindableModel, Module>(new Body
                {
                    HP = ((Body)selectedLevel.Key).HP,
                    Level = ((Body)selectedLevel.Key).Level
                }, Module.Body));

                for (int i = 0; i < Constants.NumberOfModulesInOneBody; i++)
                {
                    PlayersShipModules.Add(new KeyValuePair<IBindableModel, Module>(
                        new EmptyBody
                        {
                            HP = ((Body)selectedLevel.Key).HP,
                            Level = ((Body)selectedLevel.Key).Level
                        }, Module.EmptyBody));
                }

                buyBuffer.Add(selectedLevel);
            }
            else
            {
                int counter = -1;
                int index = -1;
                foreach (var item in PlayersShipModules)
                {
                    if (item.Value != Module.EmptyBody)
                    {
                        counter++;
                    }
                    else
                    {
                        index = counter;
                    }
                }
                if (index > 0)
                {
                    lastEmptyBody = (EmptyBody)PlayersShipModules[index + 1].Key;
                    PlayersShipModules[index + 1] = selectedLevel;
                    buyBuffer.Add(selectedLevel);
                }
            }
        }

        public void CancelLocate()
        {
            if (buyBuffer.Count > 0 && lastEmptyBody != null)
            {
                int index = PlayersShipModules.IndexOf(buyBuffer.Last());
                PlayersShipModules.Insert(index, new KeyValuePair<IBindableModel, Module>(lastEmptyBody, Module.EmptyBody));
                PlayersShipModules.Remove(buyBuffer.Last());
                buyBuffer.Remove(buyBuffer.Last());
                lastEmptyBody = null;
            }
        }
    }
}
