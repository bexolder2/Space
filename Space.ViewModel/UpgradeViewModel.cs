using Space.Helpers.Interfaces;
using Space.Infrastructure.Deserializer;
using Space.Model.BindableBase;
using Space.Model.Enums;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Space.ViewModel
{
    public class UpgradeViewModel : BindableBase
    {
        private Dictionary<IBindableModel, Module> modules;

        public UpgradeViewModel()
        {
            #region command initialization
            UpgradeCommand = new RelayCommand(OnUpgradeCommandExecuted, CanUpgradeCommandExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            #endregion

            modules = new Dictionary<IBindableModel, Module>();

            Initialize();
        }

        #region commands
        public ICommand UpgradeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        #region properties
        public Dictionary<IBindableModel, Module> Modules
        {
            get => modules;
            set => Set(ref modules, value);
        }
        #endregion

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
            //FillModules(result.Repairer, Module.Repairer);
            //FillModules(result.Storage, Module.Storage);
        }

        private void FillModules<T>(List<T> module, Module moduleType) where T : IBindableModel 
        {
            foreach(T item in module)
            {
                modules.Add(item, moduleType);
            }
        } 

        private bool CanUpgradeCommandExecute(object p) => true;
        private void OnUpgradeCommandExecuted(object p)
        {
            

        }

        private bool CanCancelCommandExecute(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {


        }
    }
}
