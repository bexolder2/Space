using Space.Helpers.Interfaces;
using Space.Infrastructure.Converters;
using Space.Infrastructure.Deserializer;
using Space.Model.BindableBase;
using Space.Model.Constants;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
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

        private Dictionary<Dictionary<IBindableModel, Module>, int> ship;
        private List<KeyValuePair<IBindableModel, Module>> playersShipModules;
        private int newSelectedModuleIndex;
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
        public int NewSelectedModuleIndex
        {
            get => newSelectedModuleIndex;
            set => Set(ref newSelectedModuleIndex, value);
        }

        public List<KeyValuePair<IBindableModel, Module>> PlayersShipModules
        {
            get => playersShipModules;
            set => Set(ref playersShipModules, value);
        }

        public Dictionary<Dictionary<IBindableModel, Module>, int> Ship
        {
            get => ship;
            set
            {
                Set(ref ship, value);
                PlayersShipModules = InitializePlayersModulesForListView();
            }
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
            Ship = new Dictionary<Dictionary<IBindableModel, Module>, int>();
            Ship = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Spaceship?.ShipModules;
        }

        private List<KeyValuePair<IBindableModel, Module>> InitializePlayersModulesForListView()
        {
            var result = new List<KeyValuePair<IBindableModel, Module>>();

            foreach(var item in Ship)
            {
                foreach(var item2 in item.Key)
                {
                    result.Add(item2);
                }
            }

            return result;
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
            foreach (var item in Ship)
            {
                shipModules.Add(item.Key.FirstOrDefault().Value, item.Value);
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
        private void OnUpgradeCommandExecuted(object p)
        {


        }

        private bool CanCancelCommandExecute(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {


        }

        private bool CanMoveCommandExecute(object p)
        {
            if (PlayersShipModules.Count > 0)
                return true;
            else return false;
        }
        private void OnMoveCommandExecuted(object p)
        {
            MessageBox.Show($"current = {NewSelectedModuleIndex}");
        }

        private bool CanNewPositionClickCommandExecute(object p) => true;
        private void OnNewPositionClickCommandExecuted(object index)
        {
            MessageBox.Show($"index = {index}");
        }
    }
}
