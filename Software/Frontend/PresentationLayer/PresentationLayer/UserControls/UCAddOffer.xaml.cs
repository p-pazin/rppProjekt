using CarchiveAPI.Dto;
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
    /// Interaction logic for UCAddOffer.xaml
    /// </summary>
    public partial class UCAddOffer : UserControl
    {
        private VehicleService vehicleService = new VehicleService();
        private OfferServices offerService = new OfferServices();
        private ContactService contactService = new ContactService();
        private List<VehicleDto> vehicles;
        public UCAddOffer()
        {
            InitializeComponent();
            infoWarning.Visibility = Visibility.Hidden;
            contactWarning.Visibility = Visibility.Hidden;
            vehicleWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
            LoadVehiclesDropDown();
            LoadContactsDropDown();
            vehicleWarning.Visibility = Visibility.Hidden;
            vehicles = new List<VehicleDto>();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCOfferCatalog());
            }
        }

        private async void LoadVehiclesDropDown()
        {
            cmbVehicles.ItemsSource = await vehicleService.GetVehiclesSale();
        }

        private async void LoadContactsDropDown()
        {
            cmbContacts.ItemsSource = await contactService.GetContactsAsync();
        }

        private void btnAddVehicle_Click(object sender, RoutedEventArgs e)
        {
            var selectedVehicle = cmbVehicles.SelectedItem as VehicleDto;
            if (selectedVehicle == null)
            {
                return;
            }

            if (vehicles.Contains(selectedVehicle))
            {
                vehicleWarning.Visibility = Visibility.Visible;
            }
            else
            {
                vehicleWarning.Visibility = Visibility.Hidden;
                vehicles.Add(selectedVehicle);
                lstSelectedVehicles.ItemsSource = null;
                lstSelectedVehicles.ItemsSource = vehicles;
            }
        }

        private void removeVehicle_Click(object sender, RoutedEventArgs e)
        {
            var selectedVehicle = lstSelectedVehicles.SelectedItem as VehicleDto;

            if (selectedVehicle != null)
            {
                vehicles.Remove(selectedVehicle);
                lstSelectedVehicles.ItemsSource = null;
                lstSelectedVehicles.ItemsSource = vehicles;
            }
        }

        private async void btnSaveOffer_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "" || txtPrice.Text == "" || dtpDateOfCreation.SelectedDate == null)
            {
                infoWarning.Visibility = Visibility.Visible;
                return;
            }
            if (cmbContacts.SelectedItem == null)
            {
                contactWarning.Visibility = Visibility.Visible;
                return;
            }
            if (vehicles.Count != 0)
            {
                contactWarning.Visibility = Visibility.Hidden;
                vehicleWarning.Visibility = Visibility.Hidden;
                var selectedContact = cmbContacts.SelectedItem as ContactDto;
                var vehicles = lstSelectedVehicles.ItemsSource as List<VehicleDto>;
                var offer = new OfferDto
                {
                    Title = txtTitle.Text,
                    Price = (double)decimal.Parse(txtPrice.Text),
                    DateOfCreation = dtpDateOfCreation.SelectedDate.Value.ToString("yyyy-MM-dd"),
                };
                try
                {
                    await offerService.PostOffer(offer, vehicles, selectedContact);
                }
                catch (Exception ex)
                {
                    warning.Visibility = Visibility.Visible;
                }

                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCOfferCatalog());
                }
            }
            else
            {
                vehicleWarning.Visibility = Visibility.Visible;
                return;
            }
        }
    }
}
