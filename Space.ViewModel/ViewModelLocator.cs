﻿using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Space.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UpgradeViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public UpgradeViewModel UpgradeViewModel => ServiceLocator.Current.GetInstance<UpgradeViewModel>();
    }
}
