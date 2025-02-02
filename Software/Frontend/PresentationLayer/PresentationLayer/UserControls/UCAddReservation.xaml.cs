using ServiceLayer.Services;
using ServiceLayer.Network.Dto;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarchiveAPI.Dto;
using System.Globalization;

namespace PresentationLayer.UserControls
{
    public partial class UCAddReservation : UserControl
    {
        private readonly ReservationService _reservationService;
        private readonly VehicleService vehicleService = new VehicleService();
        private readonly ContactService contactService = new ContactService();

        public UCAddReservation()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
            LoadVehiclesDropDown();
            LoadContactsDropDown();
        }

        private async void LoadVehiclesDropDown()
        {
            cmbVehicles.ItemsSource = await vehicleService.GetVehiclesRent();
        }

        private async void LoadContactsDropDown()
        {
            cmbContacts.ItemsSource = await contactService.GetContactsAsync();
        }

        private async void btnSaveOffer_Click(object sender, RoutedEventArgs e)
        {
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;

            if (!ValidateInputs(out int mileage, out decimal price, out DateTime startDate, out DateTime endDate, out int contactId, out int vehicleId))
            {
                infoWarning.Visibility = Visibility.Visible;
                return;
            }
            DateTime dateTime = DateTime.Now;
            var newReservation = new ReservationDto
            {
                MaxMileage = mileage,
                Price = (double)price,
                State = 0,
                StartDate = startDate.ToString("yyyy-MM-dd"),
                EndDate = endDate.ToString("yyyy-MM-dd"),
                DateOfCreation = dateTime.ToString("yyyy-MM-dd"),
                ContactId = contactId,
                VehicleId = vehicleId
            };

            try
            {
                await _reservationService.PostReservationsAsync(newReservation);

                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCReservations());
                }
            }
            catch (Exception ex)
            {
                warning.Text = "Greška: " + ex.Message;
                warning.Visibility = Visibility.Visible;
            }
        }

        private bool ValidateInputs(out int mileage, out decimal price, out DateTime startDate, out DateTime endDate, out int contactId, out int vehicleId)
        {
            mileage = 0;
            price = 0;
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
            contactId = 0;
            vehicleId = 0;

            if (string.IsNullOrWhiteSpace(txtMileage.Text) || !int.TryParse(txtMileage.Text, out mileage))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out price))
            {
                return false;
            }

            if (dtpDateOfStart.SelectedDate == null || dtpDateOfEnd.SelectedDate == null)
            {
                return false;
            }

            startDate = dtpDateOfStart.SelectedDate.Value;
            endDate = dtpDateOfEnd.SelectedDate.Value;

            if (startDate >= endDate)
            {
                return false;
            }

            if (cmbContacts.SelectedValue == null || !int.TryParse(cmbContacts.SelectedValue.ToString(), out contactId))
            {
                return false;
            }

            if (cmbVehicles.SelectedValue == null || !int.TryParse(cmbVehicles.SelectedValue.ToString(), out vehicleId))
            {
                return false;
            }

            return true;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCReservations());
            }
        }
    }
}
