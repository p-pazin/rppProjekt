using PresentationLayer.UserControls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            NavigationMenu.SelectionChanged += NavigationMenu_SelectionChanged;
            Application.Current.MainWindow = this;
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
                    case "Statistika":
                        MainContentFrame.Navigate(new UCStats());
                        break;
                    case "Ugovori":
                        MainContentFrame.Navigate(new UCContracts());
                        break;
                    case "Računi":
                        MainContentFrame.Navigate(new UCInvoices());
                        break;
                    case "Rezervacije":
                        MainContentFrame.Navigate(new UCReservations());
                        break;
                    case "Mapa vozila":
                        MainContentFrame.Navigate(new UCVehicleLocation());
                        break;
                    case "Pomoć":
                        ShowHelp();
                        break;
                    case "Oglasi":
                        MainContentFrame.Navigate(new UCAds());
                        break;
                    case "Odjava":
                        var token = new TokenManager();
                        token.ClearToken();
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            var loginWindow = new LoginWindow();
                            loginWindow.Show();
                        });
                        this.Hide();
                        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                        this.Close();
                        break;
                    default:
                        MainContentFrame.Content = null;
                        break;
                }
            }
        }

        private void ShowHelp()
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
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

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                ShowHelp();
            }
        }
    }
}