using ServiceLayer.Services;
using ServiceLayer.Network.Dto;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarchiveAPI.Dto;
using System.Collections.Generic;

namespace PresentationLayer.UserControls
{
    public partial class UCAddAd : UserControl
    {
        private readonly AdService _adService;
        private readonly VehicleService _vehicleService = new VehicleService();

        public UCAddAd()
        {
            InitializeComponent();
            _adService = new AdService();
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
            LoadBrandsDropDown();
        }

        private async void LoadBrandsDropDown()
        {
            cmbVehicles.ItemsSource = await _vehicleService.GetVehicles();
        }

        private async void btnSaveAd_Click(object sender, RoutedEventArgs e)
        {
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;

            DateTime dateTime = DateTime.Now;
            var newAd = new AdDto
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                PaymentMethod = cmbPaymentMethod.Text.ToString(),
                DateOfPublishment = dateTime.ToString("yyyy-MM-dd"),
                Brand = "brand",
                Model = "model",
                Links = new List<string>()
            };
            var vehicle = cmbVehicles.SelectedItem as VehicleDto;
            
            try
            {
                await _adService.PostAdAsync(newAd, vehicle.Id);

                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCAds());
                }
            }
            catch (Exception ex)
            {
                warning.Text = "Greška: " + ex.Message;
                warning.Visibility = Visibility.Visible;
            }
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAds());
            }
        }
    }
}
