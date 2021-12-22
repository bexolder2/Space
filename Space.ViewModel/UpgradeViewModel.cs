using Space.Helpers.Interfaces;
using Space.Infrastructure.Deserializer;
using Space.Model.BindableBase;
using Space.Model.Modules;
using Space.ViewModel.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Space.ViewModel
{
    public class UpgradeViewModel : BindableBase
    {
        private List<IBindableModel> modules;

        public UpgradeViewModel()
        {
            #region command initialization
            UpgradeCommand = new RelayCommand(OnUpgradeCommandExecuted, CanUpgradeCommandExecute);
            CancelCommand = new RelayCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            #endregion

            modules = new List<IBindableModel>();

            Initialize();
        }

        #region commands
        public ICommand UpgradeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        #region properties
        public List<IBindableModel> Modules
        {
            get => modules;
            set => Set(ref modules, value);
        }
        #endregion

        private void Initialize()
        {
            var result = JsonDeserializer.InitializeModules();
            //modules = result;
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
