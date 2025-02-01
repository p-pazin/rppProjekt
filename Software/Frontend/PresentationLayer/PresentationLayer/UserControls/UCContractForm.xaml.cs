using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCContractForm.xaml
    /// </summary>
    public partial class UCContractForm : UserControl
    {
        private int _contractType {  get; set; }
        private readonly ContractService _contractService;
        private readonly OfferServices _offerService;
        private readonly VehicleService _vehicleService;
        private readonly ContactService _contactService;
        private readonly ReservationService _reservationService;
        private readonly InsuranceService _insuranceService;

        public UCContractForm()
        {
            InitializeComponent();
            cmbOffer.SelectionChanged += cmbOffer_SelectionChanged;
            cmbVehicle.SelectionChanged += cmbVehicle_SelectionChanged;
            cmbContact.SelectionChanged += cmbContact_SelectionChanged;
            addContractWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            _contractType = 1;
            _contractService = new ContractService();
            _offerService = new OfferServices();
            _vehicleService = new VehicleService();
            _contactService = new ContactService();
            _reservationService = new ReservationService();
            _insuranceService = new InsuranceService();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var offers = await _offerService.GetOffers();
            var vehicles = await _vehicleService.GetVehicles();
            var contacts = await _contactService.GetContactsAsync();
            var reservations = await _reservationService.GetReservationsAsync();
            var insurances = await _insuranceService.GetInsurancesAsync();

            cmbOffer.ItemsSource = offers;
            cmbVehicle.ItemsSource = vehicles;
            cmbContact.ItemsSource = contacts;
            cmbReservation.ItemsSource = reservations;
            cmbInsurance.ItemsSource = insurances;

            var offerDisplayList = offers.Select(o => new
            {
                DisplayText = $"ID: {o.Id}, Title: {o.Title}",
                Offer = o
            }).ToList();
            cmbOffer.ItemsSource = offerDisplayList;
            cmbOffer.DisplayMemberPath = "DisplayText";
            cmbOffer.SelectedValuePath = "Offer";

            var vehicleDisplayList = vehicles.Select(v => new
            {
                DisplayText = $"{v.Registration}, {v.Type} {v.Model}",
                Vehicle = v
            }).ToList();
            cmbVehicle.ItemsSource = vehicleDisplayList;
            cmbVehicle.DisplayMemberPath = "DisplayText";
            cmbVehicle.SelectedValuePath = "Vehicle";

            var contactDisplayList = contacts.Select(c => new
            {
                DisplayText = $"{c.FirstName} {c.LastName}",
                Contact = c
            }).ToList();
            cmbContact.ItemsSource = contactDisplayList;
            cmbContact.DisplayMemberPath = "DisplayText";
            cmbContact.SelectedValuePath = "Contact";

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
        }
        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            grdContractRentForm.Visibility = Visibility.Collapsed;
            grdContractSaleForm.Visibility = Visibility.Visible;
            _contractType = 1;
            ResetInputs();
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            grdContractRentForm.Visibility = Visibility.Visible;
            grdContractSaleForm.Visibility = Visibility.Collapsed;
            _contractType = 2;
            ResetInputs();
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
            addContractWarning.Visibility = Visibility.Hidden;

            if (_contractType == 1) {
                bool inputsValid = ValidateInputsSale();

                if (inputsValid)
                {
                    DateTime currentDate = DateTime.Now;
                    var newContract = new ContractDto
                    {
                        Title = txtTitleSale.Text,
                        Place = txtLocationSale.Text,
                        DateOfCreation = currentDate.ToString("yyyy-MM-dd"),
                        Type = 1,
                        Content = txtContentSale.Text,
                        Signed = 0
                    };

                    try
                    {
                        dynamic selectedContact = cmbContact.SelectedItem;
                        dynamic selectedOffer = cmbOffer.SelectedItem;
                        dynamic selectedVehicle = cmbVehicle.SelectedItem;

                        int? offerId = selectedOffer?.Offer?.Id;
                        int? contactId = selectedContact?.Contact?.Id;
                        int? vehicleId = selectedVehicle?.Vehicle?.Id;

                        await _contractService.PostContractSaleAsync(newContract, contactId, vehicleId, offerId);
                        if (Application.Current.MainWindow is MainWindow mw)
                        {
                            var ucContracts = new UCContracts();
                            mw.LoadUC(ucContracts);
                            mw.AdjustUserControlMargin();
                        }
                    }
                    catch (Exception ex)
                    {
                        addContractWarning.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    infoWarning.Visibility = Visibility.Visible;
                }
            }
            else
            {
                bool inputsValid = ValidateInputsRent();

                if (inputsValid)
                {
                    DateTime currentDate = DateTime.Now;
                    var newContract = new ContractDto
                    {
                        Title = txtTitleRent.Text,
                        Place = txtLocationRent.Text,
                        DateOfCreation = currentDate.ToString("yyyy-MM-dd"),
                        Type = 2,
                        Content = txtContentRent.Text,
                        Signed = 0
                    };

                    try
                    {
                        dynamic selectedReservation = cmbReservation.SelectedItem;
                        dynamic selectedInsurance = cmbInsurance.SelectedItem;

                        int? reservationId = selectedReservation?.Reservation?.Id;
                        int? insuranceId = selectedInsurance?.Insurance?.Id;

                        await _contractService.PostContractRentAsync(newContract, reservationId, insuranceId);
                        if (Application.Current.MainWindow is MainWindow mw)
                        {
                            var ucContracts = new UCContracts();
                            mw.LoadUC(ucContracts);
                            mw.AdjustUserControlMargin();
                        }
                    }
                    catch (Exception ex)
                    {
                        addContractWarning.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    infoWarning.Visibility = Visibility.Visible;
                }
            }
        }
        private bool ValidateInputsSale()
        {
            if (txtTitleSale.Text.Length == 0 || txtContentSale.Text.Length == 0 || txtLocationSale.Text.Length == 0)
            {
                return false;
            }

            bool isOfferSelected = cmbOffer.SelectedItem != null;
            bool isContactSelected = cmbContact.SelectedItem != null;
            bool isVehicleSelected = cmbVehicle.SelectedItem != null;

            if ((isOfferSelected && (isContactSelected || isVehicleSelected)) ||
                (!isOfferSelected && !isContactSelected && !isVehicleSelected))
            {
                return false;
            }

            return true;
        }
        private bool ValidateInputsRent()
        {
            if (txtTitleRent.Text.Length == 0 || txtContentRent.Text.Length == 0 || txtLocationRent.Text.Length == 0 || 
                cmbReservation.SelectedItem == null || cmbInsurance.SelectedItem == null)
            {
                return false;
            }
            return true;
        }
        private void ResetInputs()
        {
            cmbOffer.SelectedValue = null;
            cmbContact.SelectedValue = null;
            cmbVehicle.SelectedValue = null;
            cmbInsurance.SelectedValue = null;
            cmbReservation.SelectedValue= null;
            txtTitleSale.Text = null;
            txtContentSale.Text = null;
            txtLocationSale.Text = null;
            txtTitleRent.Text = null;
            txtContentRent.Text= null;
            txtLocationRent.Text = null;
        }
        private void cmbOffer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOffer.SelectedItem != null)
            {
                cmbVehicle.SelectedItem = null;
                cmbContact.SelectedItem = null;
            }
        }

        private void cmbVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbVehicle.SelectedItem != null)
            {
                cmbOffer.SelectedItem = null;
            }
        }

        private void cmbContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbContact.SelectedItem != null)
            {
                cmbOffer.SelectedItem = null;
            }
        }
    }
}
