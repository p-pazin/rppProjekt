using CarchiveAPI.Dto;
using ServiceLayer.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    public partial class UCEditReservation : UserControl
    {
        private ReservationDto _reservation;
        private readonly VehicleService vehicleService = new VehicleService();
        private readonly ContactService contactService = new ContactService();
        private readonly ReservationService reservationService = new ReservationService();

        public UCEditReservation(ReservationDto reservation)
        {
            InitializeComponent();
            _reservation = reservation;
            PopulateFields();
            LoadVehiclesDropDown();
            LoadContactsDropDown();
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
        }

        private async void LoadVehiclesDropDown()
        {
            cmbVehicles.ItemsSource = await vehicleService.GetVehiclesRent();
        }

        private async void LoadContactsDropDown()
        {
            cmbContacts.ItemsSource = await contactService.GetContactsAsync();
        }

        private void PopulateFields()
        {
            if (_reservation != null)
            {
                txtMileage.Text = _reservation.MaxMileage.ToString();
                txtPrice.Text = _reservation.Price.ToString();

                string startDateString = _reservation.StartDate;
                DateTime parsedDateStart = DateTime.ParseExact(startDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                dtpDateOfStart.SelectedDate = parsedDateStart;

                string endDateString = _reservation.EndDate;
                DateTime parsedDateEnd = DateTime.ParseExact(endDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                dtpDateOfEnd.SelectedDate = parsedDateEnd;

                cmbContacts.SelectedValue = _reservation.ContactId;
                cmbVehicles.SelectedValue = _reservation.VehicleId;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCReservations());
            }
        }

        private async void btnSaveReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMileage.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    dtpDateOfStart.SelectedDate == null ||
                    dtpDateOfEnd.SelectedDate == null ||
                    cmbContacts.SelectedValue == null ||
                    cmbVehicles.SelectedValue == null)
                {
                    infoWarning.Visibility = Visibility.Visible;
                    return;
                }

                infoWarning.Visibility = Visibility.Hidden;

                ReservationDto updatedReservation = new ReservationDto
                {
                    Id = _reservation.Id,
                    MaxMileage = int.Parse(txtMileage.Text),
                    Price = double.Parse(txtPrice.Text),
                    DateOfCreation = _reservation.DateOfCreation,
                    StartDate = dtpDateOfStart.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    EndDate = dtpDateOfEnd.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    ContactId = (int)cmbContacts.SelectedValue,
                    VehicleId = (int)cmbVehicles.SelectedValue
                };

                await reservationService.PutReservationsAsync(updatedReservation);

                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCReservations());
                }
            }
            catch (Exception ex)
            {
                warning.Visibility = Visibility.Visible;
                MessageBox.Show($"Greška prilikom ažuriranja rezervacije: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
