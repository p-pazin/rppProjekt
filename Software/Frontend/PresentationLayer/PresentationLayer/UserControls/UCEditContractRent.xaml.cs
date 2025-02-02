using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using CarchiveAPI.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCEditContractRent.xaml
    /// </summary>
    public partial class UCEditContractRent : UserControl
    {
        private readonly ReservationService _reservationService;
        private readonly InsuranceService _insuranceService;
        private readonly ContractService _contractService;
        private int _contractId { get; set; }
        public UCEditContractRent(int contractId)
        {
            InitializeComponent();
            updateContractWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            _contractId = contractId;
            _contractService = new ContractService();
            _reservationService = new ReservationService();
            _insuranceService = new InsuranceService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var contract = await _contractService.GetContractRentAsync(_contractId);

            var reservations = await _reservationService.GetReservationsAsync();
            var insurances = await _insuranceService.GetInsurancesAsync();

            var signedStates = new List<string> { "Nepotpisan", "Potpisan" };

            cmbReservation.ItemsSource = reservations;
            cmbInsurance.ItemsSource = insurances;
            cmbSigned.ItemsSource = signedStates;

            var reservationDisplayList = reservations.Select(r => new
            {
                DisplayText = $"ID: {r.Id}, {r.StartDate} - {r.EndDate}",
                Reservation = r
            }).ToList();
            cmbReservation.ItemsSource = reservationDisplayList;
            cmbReservation.DisplayMemberPath = "DisplayText";
            cmbReservation.SelectedValuePath = "Reservation";

            var insuranceDisplayList = insurances.Select(i => new
            {
                DisplayText = $"{i.Name}",
                Insurance = i
            }).ToList();
            cmbInsurance.ItemsSource = insuranceDisplayList;
            cmbInsurance.DisplayMemberPath = "DisplayText";
            cmbInsurance.SelectedValuePath = "Insurance";

            if (contract != null)
            {
                if (contract.ReservationId > 0)
                {
                    var selectedReservation = reservations.FirstOrDefault(r => r.Id == contract.ReservationId);
                    if (selectedReservation != null)
                        cmbReservation.SelectedItem = reservationDisplayList.FirstOrDefault(r => r.Reservation == selectedReservation);
                }

                if (contract.NameInsurance != null)
                {
                    var selectedInsurance = insurances.FirstOrDefault(i => i.Name == contract.NameInsurance);
                    if (selectedInsurance != null)
                        cmbInsurance.SelectedItem = insuranceDisplayList.FirstOrDefault(i => i.Insurance == selectedInsurance);
                }

                cmbSigned.SelectedItem = contract.Signed == 1 ? "Potpisan" : "Nepotpisan";
                txtTitleRent.Text = contract.Title;
                txtContentRent.Text = contract.Content;
                txtLocationRent.Text = contract.Place;
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCContracts());
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            infoWarning.Visibility = Visibility.Hidden;
            updateContractWarning.Visibility = Visibility.Hidden;

            bool inputsValid = ValidateInputs();

            if (inputsValid)
            {
                DateTime currentDate = DateTime.Now;
                var newContract = new ContractDto
                {
                    Title = txtTitleRent.Text,
                    Place = txtLocationRent.Text,
                    Type = 2,
                    Content = txtContentRent.Text,
                    Signed = cmbSigned.SelectedValue?.ToString() == "Potpisan" ? 1 : 0,
                    Id = _contractId,
                    DateOfCreation = currentDate.ToString("yyyy-MM-dd")
                };

                try
                {
                    dynamic selectedReservation = cmbReservation.SelectedItem;
                    dynamic selectedInsurance = cmbInsurance.SelectedItem;

                    int? reservationId = selectedReservation?.Reservation?.Id;
                    int? insuranceId = selectedInsurance?.Insurance?.Id;

                    await _contractService.PutContractRentAsync(newContract, reservationId, insuranceId);
                    if (Application.Current.MainWindow is MainWindow mw)
                    {
                        var ucContracts = new UCContracts();
                        mw.LoadUC(ucContracts);
                        mw.AdjustUserControlMargin();
                    }
                }
                catch (Exception ex)
                {
                    updateContractWarning.Visibility = Visibility.Visible;
                }
            }
            else
            {
                infoWarning.Visibility = Visibility.Visible;
            }
        }
        private bool ValidateInputs()
        {
            if (txtTitleRent.Text.Length == 0 || txtContentRent.Text.Length == 0 || txtLocationRent.Text.Length == 0 ||
                cmbReservation.SelectedItem == null || cmbInsurance.SelectedItem == null)
            {
                return false;
            }
            return true;
        }
    }
}
