using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for UCEditOffer.xaml
    /// </summary>
    public partial class UCEditOffer : UserControl
    {
        private VehicleService vehicleService = new VehicleService();
        private OfferServices offerService = new OfferServices();
        private ContactService contactService = new ContactService();
        private int offerId;
        List<VehicleDto>  vehicles = new List<VehicleDto>();
        public UCEditOffer(OfferDto offer)
        {
            InitializeComponent();
            vehicleWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
            LoadOfferInfo(offer);
            LoadContactsDropDown();
            LoadVehiclesDropDown();
            LoadVehiclesForOffer();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCOfferCatalog());
            }
        }

        private void LoadOfferInfo(OfferDto offer)
        {
            txtTitle.Text = offer.Title;
            txtPrice.Text = offer.Price.ToString();
            offerId = offer.Id;

            string dateString = offer.DateOfCreation;
            DateTime parsedDate = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            dtpDateOfCreation.SelectedDate = parsedDate;
        }

        private async void LoadVehiclesDropDown()
        {
            cmbVehicles.ItemsSource = await vehicleService.GetVehiclesSale();
        }

        private async void LoadContactsDropDown()
        {
            cmbContacts.ItemsSource = await contactService.GetContactsAsync();
            var contact = await offerService.GetContactForOffer(offerId);
            cmbContacts.SelectedValue = contact.Id;
        }

        private async void LoadVehiclesForOffer()
        {
            vehicles = await offerService.GetVehiclesForOffer(offerId);
            lstSelectedVehicles.ItemsSource = vehicles;
        }

        private async void btnSaveOffer_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "" || txtPrice.Text == "" || dtpDateOfCreation.SelectedDate == null)
            {
                infoWarning.Visibility = Visibility.Visible;
                return;
            }
            var vehicles = lstSelectedVehicles.ItemsSource as List<VehicleDto>;
            if (vehicles.Count == 0)
            {
                vehicleWarning.Visibility = Visibility.Visible;
                return;
            }
            var selectedContact = cmbContacts.SelectedItem as ContactDto;
            if (selectedContact == null)
            {
                return;
            }
            var offer = new OfferDto
            {
                Id = offerId,
                Title = txtTitle.Text,
                Price = (double)decimal.Parse(txtPrice.Text),
                DateOfCreation = dtpDateOfCreation.SelectedDate.Value.ToString("yyyy-MM-dd"),
            };
            try
            {
                await offerService.PutOffer(offer, vehicles, selectedContact);
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

        private void removeVehicle_Click(object sender, RoutedEventArgs e)
        {
            var selectedVehicle = lstSelectedVehicles.SelectedItem as VehicleDto;
            if (selectedVehicle != null)
            {
                vehicles.Remove(selectedVehicle);
                lstSelectedVehicles.Items.Refresh();
            }
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
                lstSelectedVehicles.Items.Refresh();
            }
        }
    }
}
