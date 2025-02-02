using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCVehicleCatalog.xaml
    /// </summary>
    public partial class UCVehicleCatalog : UserControl
    {
        private VehicleService vehicleService = new VehicleService();
        public UCVehicleCatalog()
        {
            InitializeComponent();
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.AdjustUserControlMargin();
            }
            LoadVehiclesAsync();
        }

        private async void LoadVehiclesAsync()
        {
            dgvVehicles.ItemsSource = await vehicleService.GetNotDeletedVehicles();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddVehicle());
                mw.AdjustUserControlMargin();
            }
        }

        private void btnEditvehicle_Click(object sender, RoutedEventArgs e)
        {
            var selectedVehicle = dgvVehicles.SelectedItem as VehicleDto;

            if (selectedVehicle != null) {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCEditVehicle(selectedVehicle));
                    mw.AdjustUserControlMargin();
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private async void btnDeleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedVehicle = dgvVehicles.SelectedItem as VehicleDto;
            if (selectedVehicle != null)
            {
                var messageBox = new DeletionWarningWindow("vozilo");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        await vehicleService.DeleteVehicle(selectedVehicle.Id);
                        LoadVehiclesAsync();
                    }
                    catch (Exception ex)
                    {
                        deletionWarning.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

    }
}
