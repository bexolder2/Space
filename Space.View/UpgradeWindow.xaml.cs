﻿using Space.Model.Enums;
using Space.ViewModel;
using System.Windows;

namespace Space.View
{
    public partial class UpgradeWindow : Window
    {
        public UpgradeWindow()
        {
            InitializeComponent();
            SetWindowState(WndAction.Show);
        }

        private void SetWindowState(WndAction action)
        {
            switch (action)
            {
                case WndAction.Hide:
                    Hide();
                    break;
                case WndAction.Show:
                    Show();
                    break;
                default:
                    break;
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
