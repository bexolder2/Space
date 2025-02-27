﻿using GalaSoft.MvvmLight.Messaging;
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
            buyBuffer = new List<KeyValuePair<IBindableModel, Module>>();

            Initialize();
            InitializeSelectedModules();
        }

        #region commands
        public ICommand UpgradeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand NewPositionClickCommand { get; private set; }
        public ICommand BuyCommand { get; private set; }
        #endregion

        #region properties
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
            var player = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player;
            var spaceship = player?.Spaceship?.ShipModules;
            PlayersShipModules = new List<KeyValuePair<IBindableModel, Module>>();
            Bodies = new List<KeyValuePair<IBindableModel, Module>>();

            if (spaceship == null || spaceship.Count == 0)
            {
                PlayersShipModules = Constants.BaseComplectation;
            }
            else
            {
                PlayersShipModules = spaceship;
            }     

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

        private bool ValidationByPrice(KeyValuePair<IBindableModel, Module> module)
        {
            bool result = false;

            double budget = (double)(Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player?.Resources?.CryptocurrencyValue;
            if (budget - ((BaseModel)(module.Key)).Price >= 0)
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

            if (index >= 0 && PlayersShipModules.Count > index)
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

        private bool Validate(KeyValuePair<IBindableModel, Module> module)
        {
            bool result = false;
            System.Diagnostics.Debug.WriteLine("Validate");
            if (ValidateNumberOfCommandCenters())
            {
                if (ValidateNumberOfModules())
                {
                    if (ValidateHP())
                    {
                        if (ValidationByPrice(module))
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

        private bool CanUpgradeCommandExecute(object parameter)
        {
            bool result = false;
            if (parameter != null && parameter.ToString() == "body")
            {
                result = true;
            }
            else
            {
                if (selectedModule.Key != null)
                {
                    if (selectedModuleIndex >= 0)
                    {
                        if (((BaseModel)PlayersShipModules[selectedModuleIndex].Key).Level != Level.Third)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        private void OnUpgradeCommandExecuted(object _)
        {
            KeyValuePair<IBindableModel, Module> selected = new KeyValuePair<IBindableModel, Module>();
            if (selectedModuleIndex >= 0)
            {
                selected = PlayersShipModules[selectedModuleIndex];
            }
            if (selected.Key != null)
            {
                KeyValuePair<IBindableModel, Module> newModule = new KeyValuePair<IBindableModel, Module>();
                KeyValuePair<IBindableModel, Module> body = new KeyValuePair<IBindableModel, Module>();

                if (selected.Value == Module.EmptyBody)
                {
                    if (Bodies.Count > 0)
                    {
                        body = Bodies.First(x => ((BaseModel)x.Key).Level != Level.Third);
                        if (body.Key != null)
                        {
                            switch (((BaseModel)body.Key).Level)
                            {
                                case Level.First:
                                    newModule = Modules.FirstOrDefault(x => ((BaseModel)x.Key).Level == Level.Second && x.Value == body.Value);
                                    break;
                                case Level.Second:
                                    newModule = Modules.FirstOrDefault(x => ((BaseModel)x.Key).Level == Level.Third && x.Value == body.Value);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    switch (((BaseModel)selected.Key).Level)
                    {
                        case Level.First:
                            newModule = Modules.FirstOrDefault(x => ((BaseModel)x.Key).Level == Level.Second && x.Value == selected.Value);
                            break;
                        case Level.Second:
                            newModule = Modules.FirstOrDefault(x => ((BaseModel)x.Key).Level == Level.Third && x.Value == selected.Value);
                            break;
                    }
                }

                if (ValidationByPrice(newModule))
                {
                    var player = (Application.Current.Resources["Locator"] as ViewModelLocator)?.MainViewModel?.Player;

                    if (selected.Value == Module.EmptyBody)
                    {
                        var localBodies = Modules.Where(x => x.Value == Module.Body).ToList(); //list of bodies 1-2-3 lvl
                        var localBody = localBodies.FirstOrDefault(x => ((BaseModel)x.Key).Level == ((BaseModel)body.Key).Level).Key; //necessary body
                        player.Resources.CryptocurrencyValue -= ((BaseModel)localBody).Price; //price for neccessary lvl
                        int index = Bodies.IndexOf(body); //replace body
                        Bodies.RemoveAt(index);
                        Bodies.Insert(index, newModule);
                    }
                    else
                    {
                        player.Resources.CryptocurrencyValue -= ((BaseModel)selected.Key).Price;
                        int index = PlayersShipModules.IndexOf(selected);
                        PlayersShipModules.RemoveAt(index);
                        PlayersShipModules.Insert(index, newModule);
                    }

                    Messenger.Default.Send(true);
                    MessageBox.Show($"Модуль {newModule.Value} улучшен до {(int)((BaseModel)newModule.Key).Level + 1} уровня.");
                    CheckWin();
                }
                else MessageBox.Show($"Недостаточно крипты для улучшения");
            }
        }

        private void CheckWin()
        {
            if (((BaseModel)playersShipModules.FirstOrDefault(x => x.Value == Module.CommandCenter).Key).Level == Level.Third)
            {
                if (Bodies.Count == 12)
                {
                    List<bool> resultList = new List<bool>();
                    bool result = false;

                    foreach(var module in playersShipModules)
                    {
                        resultList.Add(((BaseModel)module.Key).Level == Level.Third);
                    }

                    foreach(var body in Bodies)
                    {
                        resultList.Add(((BaseModel)body.Key).Level == Level.Third);
                    }

                    foreach(var item in resultList)
                    {
                        result = result && item; 
                    }

                    if (result)
                    {
                        MessageBox.Show("You are win!");
                        Messenger.Default.Send("win");
                    }
                }
            }
        }

        private bool CanCancelCommandExecute(object _) => true;
        private void OnCancelCommandExecuted(object _)
        {
            foreach(var item in Bodies)
            {
                PlayersShipModules.Add(item);
            }
            List<KeyValuePair<IBindableModel, Module>> result = new List<KeyValuePair<IBindableModel, Module>>();
            if (buyBuffer.Count > 0)
            {
                result = PlayersShipModules.Except(buyBuffer).ToList();
            }
            else result = PlayersShipModules;

            Messenger.Default.Send(result);
            Messenger.Default.Send(true);
        }

        private bool CanMoveCommandExecute(object _)
        {
            if (PlayersShipModules.Count > 0)
                return true;
            else return false;
        }
        private void OnMoveCommandExecuted(object _)
        {
            startIndex = selectedModuleIndex;
        }

        #region modules moving
        private bool CanNewPositionClickCommandExecute(object _) => true;
        private void OnNewPositionClickCommandExecuted(object index)
        {
            if(index != null && index is int)
            {
                finishIndex = (int)index;

                if(startIndex >= 0 && finishIndex >= 0)
                {
                    SwapItems(startIndex, finishIndex);
                    if (!ValidateLocation(finishIndex))
                    {
                        SwapItems(startIndex, finishIndex);
                        startIndex = finishIndex = -1;
                        MessageBox.Show("Неприемлемое размещение модуля.");
                    }
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
            List<KeyValuePair<IBindableModel, Module>> buyBufferCopy = new List<KeyValuePair<IBindableModel, Module>>();
            foreach (var item in buyBuffer)
            {
                buyBufferCopy.Add(item);
            }

            foreach (var module in buyBufferCopy)
            {
                if (module.Value == Module.Body)
                {
                    if (player.Spaceship.ShipModules.Count == 4)
                    {
                        List<bool> tmp = new List<bool>();
                        foreach (var item in player.Spaceship.ShipModules)
                        {
                            if (item.Value == Module.EmptyBody)
                            {
                                tmp.Add(true);
                            }
                            else tmp.Add(false);
                        }
                        bool emptyFlag = true;
                        foreach (var item in tmp)
                        {
                            emptyFlag = emptyFlag && item;
                        }

                        if (emptyFlag)
                        {
                            Buy(player, module);
                        }
                    }
                    else
                    {
                        if (Validate(module))
                        {
                            Buy(player, module);
                        }
                    }
                }
                else
                {
                    if (Validate(module))
                    {
                        if (ValidateLocation(PlayersShipModules.IndexOf(module)))
                        {
                            Buy(player, module);
                        }
                        else MessageBox.Show("Недопустимое расположение");
                    }
                }
            }
            Messenger.Default.Send(true);
        }

        private void Buy(Player player, KeyValuePair<IBindableModel, Module> module)
        {
            KeyValuePair<IBindableModel, Module> newModule = new KeyValuePair<IBindableModel, Module>();
            if (((BaseModel)module.Key).Level == Level.First)
            {
                newModule = module;
            }
            else
            {
                var currentlySelectedModule = Modules.Where(item => item.Value == module.Value)
                                                     .ToDictionary(_key => _key.Key, _value => _value.Value);
                newModule = currentlySelectedModule.FirstOrDefault();   
            }

            if (module.Value == Module.Body)
            {
                buyBuffer.Remove(module);
                PlayersShipModules.Remove(module);
                player.Resources.CryptocurrencyValue -= ((BaseModel)newModule.Key).Price;
            }
            else
            {
                int index = PlayersShipModules.IndexOf(module);
                buyBuffer.Remove(module);
                PlayersShipModules.Remove(module);
                PlayersShipModules.Insert(index, new KeyValuePair<IBindableModel, Module>(newModule.Key, newModule.Value));
                player.Resources.CryptocurrencyValue -= ((BaseModel)newModule.Key).Price;
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
                int index = PlayersShipModules.IndexOf(PlayersShipModules.FirstOrDefault(x => x.Value == Module.EmptyBody));
                if (index >= 0)
                {
                    if (PlayersShipModules[index].Value == Module.EmptyBody)
                    {
                        lastEmptyBody = (EmptyBody)PlayersShipModules[index].Key;
                        PlayersShipModules[index] = selectedLevel;
                        buyBuffer.Add(selectedLevel);
                    }
                    else MessageBox.Show("Выберите модификацию первого уровня.");
                }
            }
        }

        public void CancelLocate()
        {
            if (buyBuffer.Count > 0)
            {
                if (buyBuffer.Last().Value == Module.Body)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        PlayersShipModules.RemoveAt(PlayersShipModules.Count - 1);
                    }
                    buyBuffer.Remove(buyBuffer.Last());
                    lastEmptyBody = null;
                }
                else
                {
                    if (lastEmptyBody != null)
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
    }
}