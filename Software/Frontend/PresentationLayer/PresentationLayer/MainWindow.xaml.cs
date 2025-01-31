﻿using PresentationLayer.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationMenu.SelectionChanged += NavigationMenu_SelectionChanged;

            NavigationMenu.SelectedIndex = 0;
            ToggleDrawerButton.Content = "✕";

            this.SizeChanged += MainWindow_SizeChanged;

            Drawer.IsVisibleChanged += Drawer_IsVisibleChanged;
        }

        private void NavigationMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationMenu.SelectedItem is ListBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Nadzorna ploča":
                        MainContentFrame.Navigate(new UCDashboard());
                        break;
                    case "Katalog vozila":
                        MainContentFrame.Navigate(new UCVehicleCatalog());
                        break;
                    case "Ponude":
                        MainContentFrame.Navigate(new UCOfferCatalog());
                        break;
                    case "Popis kontakata":
                        MainContentFrame.Navigate(new UCContacts());
                        break;
                    case "Ugovori":
                        MainContentFrame.Navigate(new UCContracts());
                        break;
                    case "Odjava":
                        var loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                        var token = new TokenManager();
                        token.ClearToken();
                        break;
                    default:
                        MainContentFrame.Content = null;
                        break;
                }
            }
        }

        private void ToggleDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            if (Drawer.Visibility == Visibility.Visible)
            {
                Drawer.Visibility = Visibility.Collapsed;
                ToggleDrawerButton.Content = "☰";
                ToggleDrawerButton.Margin = new Thickness(-210, 10, 0, 0);
            }
            else
            {
                Drawer.Visibility = Visibility.Visible;
                ToggleDrawerButton.Content = "✕";
                ToggleDrawerButton.Margin = new Thickness(10, 10, 0, 0);
            }

            AdjustUserControlMargin();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustUserControlMargin();
        }

        private void Drawer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AdjustUserControlMargin();
        }

        public void AdjustUserControlMargin()
        {
            if (MainContentFrame.Content is UserControl uc)
            {
                if (Drawer.Visibility == Visibility.Visible)
                {
                    uc.Margin = new Thickness(0);
                }
                else
                {
                    uc.Margin = new Thickness(-220, 0, 0, 0);
                }
            }
        }

        public void LoadUC(UserControl uc)
        {
            MainContentFrame.Content = uc;
            AdjustUserControlMargin();
        }
    }
}