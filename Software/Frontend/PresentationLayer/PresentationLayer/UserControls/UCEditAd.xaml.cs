using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    public partial class UCEditAd : UserControl
    {
        private readonly AdDto _ad;
        private readonly AdService _adService = new AdService();
        private readonly VehicleService _vehicleService = new VehicleService();

        public UCEditAd(AdDto ad)
        {
            InitializeComponent();
            _ad = ad;
            PopulateFields();
            infoWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
        }

        private void PopulateFields()
        {
            if (_ad != null)
            {
                txtTitle.Text = _ad.Title;
                txtDescription.Text = _ad.Description;
                dtpDateOfPublishment.SelectedDate = DateTime.ParseExact(_ad.DateOfPublishment, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                txtVehicle.Text = $"{_ad.Brand} {_ad.Model}";

                switch (_ad.PaymentMethod)
                {
                    case "Gotovina":
                        cmbPaymentMethod.SelectedIndex = 0;
                        break;
                    case "Kartica":
                        cmbPaymentMethod.SelectedIndex = 1;
                        break;
                    case "Kripto":
                        cmbPaymentMethod.SelectedIndex = 2;
                        break;
                    case "Kredit":
                        cmbPaymentMethod.SelectedIndex = 3;
                        break;
                    case "Uplata na račun":
                        cmbPaymentMethod.SelectedIndex = 4;
                        break;
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAds());
            }
        }

        private async void btnSaveAd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                    string.IsNullOrWhiteSpace(txtDescription.Text) ||
                    dtpDateOfPublishment.SelectedDate == null ||
                    cmbPaymentMethod.SelectedItem == null)
                {
                    infoWarning.Visibility = Visibility.Visible;
                    return;
                }

                infoWarning.Visibility = Visibility.Hidden;

                AdDto updatedAd = new AdDto
                {
                    Id = _ad.Id,
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    DateOfPublishment = dtpDateOfPublishment.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    PaymentMethod = ((ComboBoxItem)cmbPaymentMethod.SelectedItem).Content.ToString(),
                    Model = _ad.Model,
                    Brand = _ad.Brand,
                    Links = _ad.Links
                };

                await _adService.PutReservationsAsync(updatedAd);

                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCAds());
                }
            }
            catch (Exception ex)
            {
                warning.Visibility = Visibility.Visible;
                MessageBox.Show($"Greška prilikom ažuriranja oglasa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
