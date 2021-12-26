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
        private List<Body> bodies;
        private int selectedModuleIndex;
        private int startIndex, finishIndex;
        private IBindableModelToModelTextConverter modelConverter = new IBindableModelToModelTextConverter();

        public UpgradeViewModel()
        {
            #region command initialization
            UpgradeCommand = new RelayCommand(OnUpgradeCommandExecuted, CanUpgradeCommandExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            MoveCommand = new RelayCommand(OnMoveCommandExecuted, CanMoveCommandExecute);
            NewPositionClickCommand = new RelayCommand(OnNewPositionClickCommandExecuted, CanNewPositionClickCommandExecute);
            #endregion

            modules = new Dictionary<IBindableModel, Module>();
            PlayersShipModules = new List<KeyValuePair<IBindableModel, Module>>();

            Initialize();
            InitializeSelectedModules();
            InitializeShip();
        }

        #region commands
        public ICommand UpgradeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand NewPositionClickCommand { get; private set; }
        #endregion

        #region properties
        public List<Body> Bodies
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

        private void InitializeShip()
        {
            PlayersShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            PlayersShipModules = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Spaceship?.ShipModules;
            Bodies = new List<Body>();

            foreach(var item in PlayersShipModules)
            {
                if(item.Value is Module.Body)
                {
                    Bodies.Add((Body)item.Key);
                    Bodies.LastOrDefault().Index = PlayersShipModules.IndexOf(item);
                }
            }

            for (int i = 0; i < Bodies.Count; i++)
            {
                if (i > 0)
                {
                    PlayersShipModules.RemoveAt(Bodies[i].Index - 1);
                }
                else PlayersShipModules.RemoveAt(Bodies[i].Index);
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
            int bodies = shipModules.FirstOrDefault(x => x.Key == Module.Body).Value;
            int engines = shipModules.FirstOrDefault(x => x.Key == Module.Engine).Value;

            if (bodies / 2 == engines)
                result = true;

            return result;
        }

        private bool ValidateNumberOfBodies()
        {
            bool result = false;
            Dictionary<Module, int> shipModules = GetShipModules();
            int bodies = shipModules.FirstOrDefault(x => x.Key == Module.Body).Value;

            if (bodies * 4 >= CalculateTotalNumbersOfModules())
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

            if (PlayersShipModules.Count >= index)
            {
                if(PlayersShipModules.Count >= index + 1)
                {
                    compatibilities.Add(CheckModulesCompatibility(PlayersShipModules[index].Value, 
                                        PlayersShipModules[index + 1].Value));
                }
                if (PlayersShipModules.Count >= index - 1)
                {
                    compatibilities.Add(CheckModulesCompatibility(PlayersShipModules[index].Value, 
                                        PlayersShipModules[index - 1].Value));
                }
                if (PlayersShipModules.Count >= index + Constants.NumberOfModulesInOneBody)
                {
                    compatibilities.Add(CheckModulesCompatibility(PlayersShipModules[index].Value, 
                                        PlayersShipModules[index + Constants.NumberOfModulesInOneBody].Value));
                }
                if (PlayersShipModules.Count >= index - Constants.NumberOfModulesInOneBody)
                {
                    compatibilities.Add(CheckModulesCompatibility(PlayersShipModules[index].Value, 
                                        PlayersShipModules[index - Constants.NumberOfModulesInOneBody].Value));
                }
            }

            foreach(var item in compatibilities)
            {
                result = result && item;
            }

            return result;
        }
        #endregion

        private bool ValidateHP()
        {
            bool result = false;

            int currentHP = (int)(Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Spaceship?.HP;
            if (currentHP + ((BaseModel)SelectedLevel.Key).HP > 0)
                result = true;

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

                if(newModule.Value is Module.Body)
                {
                    Bodies.Add(new Body
                               {
                                   HP = ((Body)newModule.Key).HP,
                                   Level = ((Body)newModule.Key).Level,
                                   Index = PlayersShipModules.Count
                               });

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
            }
        }

        private bool CanCancelCommandExecute(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {
            foreach(var item in Bodies)
            {
                PlayersShipModules.Insert(item.Index, new KeyValuePair<IBindableModel, Module>(item, Module.Body));
            }

            Messenger.Default.Send(PlayersShipModules);
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

        private bool CanNewPositionClickCommandExecute(object p) => true;
        private void OnNewPositionClickCommandExecuted(object index)
        {
            if(index != null && index is int)
            {
                finishIndex = (int)index;

                if(startIndex > 0 && finishIndex > 0)
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
    }
}
