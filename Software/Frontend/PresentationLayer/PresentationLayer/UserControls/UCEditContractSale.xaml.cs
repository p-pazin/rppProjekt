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
using CarchiveAPI.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCEditContractSale.xaml
    /// </summary>
    public partial class UCEditContractSale : UserControl
    {
        private readonly ContractService _contractService;
        private readonly OfferServices _offerService;
        private readonly VehicleService _vehicleService;
        private readonly ContactService _contactService;
        private int _contractId {  get; set; }
        public UCEditContractSale(int contractId)
        {
            InitializeComponent();
            cmbOffer.SelectionChanged += cmbOffer_SelectionChanged;
            cmbVehicle.SelectionChanged += cmbVehicle_SelectionChanged;
            cmbContact.SelectionChanged += cmbContact_SelectionChanged;
            updateContractWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            _contractId = contractId;
            _contractService = new ContractService();
            _offerService = new OfferServices();
            _vehicleService = new VehicleService();
            _contactService = new ContactService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var contract = await _contractService.GetContractSaleAsync(_contractId);

            var offers = await _offerService.GetOffers();
            var vehicles = await _vehicleService.GetVehicles();
            var contacts = await _contactService.GetContactsAsync();

            var signedStates = new List<string> { "Nepotpisan", "Potpisan" };

            cmbOffer.ItemsSource = offers;
            cmbVehicle.ItemsSource = vehicles;
            cmbContact.ItemsSource = contacts;
            cmbSigned.ItemsSource = signedStates;

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

            if (contract != null)
            {
                if (contract.OfferId.HasValue)
                {
                    var selectedOffer = offers.FirstOrDefault(o => o.Id == contract.OfferId.Value);
                    if (selectedOffer != null)
                        cmbOffer.SelectedItem = offerDisplayList.FirstOrDefault(o => o.Offer == selectedOffer);
                }

                if (contract.Vehicle != null)
                {
                    var selectedVehicle = vehicles.FirstOrDefault(v => v.Id == contract.Vehicle.Id);
                    if (selectedVehicle != null)
                        cmbVehicle.SelectedItem = vehicleDisplayList.FirstOrDefault(v => v.Vehicle == selectedVehicle);
                }

                if (!string.IsNullOrEmpty(contract.ContactName))
                {
                    var selectedContact = contacts.FirstOrDefault(c => $"{c.FirstName} {c.LastName}" == contract.ContactName);
                    if (selectedContact != null)
                        cmbContact.SelectedItem = contactDisplayList.FirstOrDefault(c => c.Contact == selectedContact);
                }

                cmbSigned.SelectedItem = contract.Signed == 1 ? "Potpisan" : "Nepotpisan";
                txtTitleSale.Text = contract.Title;
                txtContentSale.Text = contract.Content;
                txtLocationSale.Text = contract.Place;
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
                    Id = _contractId,
                    Title = txtTitleSale.Text,
                    Place = txtLocationSale.Text,
                    Type = 1,
                    Content = txtContentSale.Text,
                    Signed = cmbSigned.SelectedValue?.ToString() == "Potpisan" ? 1 : 0,
                    DateOfCreation = currentDate.ToString("yyyy-MM-dd")
                };

                try
                {
                    dynamic selectedContact = cmbContact.SelectedItem;
                    dynamic selectedOffer = cmbOffer.SelectedItem;
                    dynamic selectedVehicle = cmbVehicle.SelectedItem;

                    int? offerId = selectedOffer?.Offer?.Id;
                    int? contactId = selectedContact?.Contact?.Id;
                    int? vehicleId = selectedVehicle?.Vehicle?.Id;

                    await _contractService.PutContractSaleAsync(newContract, contactId, vehicleId, offerId);
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
