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
using BruTile.Wmts.Generated;
using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCADDInvoice.xaml
    /// </summary>
    public partial class UCAddInvoice : UserControl
    {
        private readonly InvoiceService invoiceService = new InvoiceService();
        private readonly ContractService contractService = new ContractService();
        private readonly PenaltyService penaltyService = new PenaltyService();
        private List<PenaltyDto> penalties;
        public UCAddInvoice()
        {
            InitializeComponent();
            penaltyWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            addContractWarning.Visibility = Visibility.Hidden;
            LoadContracts();
            LoadPenalties();
            penalties = new List<PenaltyDto>();
        }

        private async void LoadContracts()
        {
            try
            {
                var contracts = await contractService.GetContractsAsync();
                var rentContracts = contracts.FindAll(c => c.Type == 2);
                var saleContracts = contracts.FindAll(c => c.Type == 1);

                cmbContractSale.ItemsSource = saleContracts;

                var contractSaleDisplayList = saleContracts.Select(v => new
                {
                    DisplayText = $"{v.Title}",
                    Contract = v
                }).ToList();
                cmbContractSale.ItemsSource = contractSaleDisplayList;
                cmbContractSale.DisplayMemberPath = "DisplayText";
                cmbContractSale.SelectedValuePath = "Contract";

                cmbContractStart.ItemsSource = rentContracts;

                var contractStartDisplayList = rentContracts.Select(v => new
                {
                    DisplayText = $"{v.Title}",
                    Contract = v
                }).ToList();
                cmbContractStart.ItemsSource = contractStartDisplayList;
                cmbContractStart.DisplayMemberPath = "DisplayText";
                cmbContractStart.SelectedValuePath = "ContractStart";

                cmbContractEnd.ItemsSource = rentContracts;

                var contractEndDisplayList = rentContracts.Select(v => new
                {
                    DisplayText = $"{v.Title}",
                    Contract = v
                }).ToList();
                cmbContractEnd.ItemsSource = contractEndDisplayList;
                cmbContractEnd.DisplayMemberPath = "DisplayText";
                cmbContractEnd.SelectedValuePath = "ContractEnd";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja ugovora: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadPenalties()
        {
            try
            {
                var penalties = await penaltyService.GetPenaltiesAsync();
                cmbPenalties.ItemsSource = penalties;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja kazni: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddPenalty_Click(object sender, RoutedEventArgs e)
        {
            var selectedPenalty = cmbPenalties.SelectedItem as PenaltyDto;
            if (selectedPenalty == null)
            {
                return;
            }

            if (penalties.Contains(selectedPenalty))
            {
                penaltyWarning.Visibility = Visibility.Visible;
            }
            else
            {
                penaltyWarning.Visibility = Visibility.Hidden;
                penalties.Add(selectedPenalty);
                lstSelectedPenalties.ItemsSource = null;
                lstSelectedPenalties.ItemsSource = penalties;
            }
        }

        private void removePenalty_Click(object sender, RoutedEventArgs e)
        {
            var selectedPenalty = lstSelectedPenalties.SelectedItem as PenaltyDto;

            if (selectedPenalty != null)
            {
                penalties.Remove(selectedPenalty);
                lstSelectedPenalties.ItemsSource = null;
                lstSelectedPenalties.ItemsSource = penalties;
            }
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            grdContractRentStartForm.Visibility = Visibility.Collapsed;
            grdContractRentEndForm.Visibility = Visibility.Collapsed;
            grdContractSaleForm.Visibility = Visibility.Visible;
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            grdContractRentStartForm.Visibility = Visibility.Visible;
            grdContractRentEndForm.Visibility = Visibility.Collapsed;
            grdContractSaleForm.Visibility = Visibility.Collapsed;
        }

        private void cmbContractStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectecContract = cmbContractStart.SelectedItem;
            if (selectecContract != null)
            {
                if (selectecContract.Contract.Signed == 1)
                {
                    grdContractRentStartForm.Visibility = Visibility.Collapsed;
                    grdContractRentEndForm.Visibility = Visibility.Visible;
                    grdContractSaleForm.Visibility = Visibility.Collapsed;
                    cmbContractEnd.SelectedItem = null;

                }
            }
        }

        private void cmbContractEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectecContract = cmbContractEnd.SelectedItem;
            if (selectecContract != null)
            {
                if (selectecContract.Contract.Signed == 0)
                {
                    grdContractRentStartForm.Visibility = Visibility.Visible;
                    grdContractRentEndForm.Visibility = Visibility.Collapsed;
                    grdContractSaleForm.Visibility = Visibility.Collapsed;
                    cmbContractStart.SelectedItem = null;
                }
            }

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdContractSaleForm.Visibility == Visibility.Visible)
                {
                    if (cmbContractSale.SelectedItem == null || cmbPaymenthSale.SelectedItem == null || string.IsNullOrEmpty(txtVAT.Text) || string.IsNullOrEmpty(txtPriceSale.Text))
                    {
                        infoWarning.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else if (grdContractRentStartForm.Visibility == Visibility.Visible)
                {
                    if (cmbContractStart.SelectedItem == null || cmbPaymenthStart.SelectedItem == null || string.IsNullOrEmpty(txtVATStart.Text))
                    {
                        infoWarning.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else if (grdContractRentEndForm.Visibility == Visibility.Visible)
                {
                    if (cmbContractEnd.SelectedItem == null || cmbPaymenthEnd.SelectedItem == null || string.IsNullOrEmpty(txtVATEnd.Text) || string.IsNullOrEmpty(txtMilageEnd.Text))
                    {
                        infoWarning.Visibility = Visibility.Visible;
                        return;
                    }
                }


                var newInvoice = new InvoiceDto();
                DateTime dateTime = DateTime.Now;
                newInvoice.DateOfCreation = dateTime.ToString("yyyy-MM-dd");

                if (grdContractSaleForm.Visibility == Visibility.Visible)
                {
                    dynamic selectedContract = cmbContractSale.SelectedItem;
                    newInvoice.ContractId = selectedContract.Contract.Id;
                    newInvoice.PaymentMethod = cmbPaymenthSale.Text.ToString();
                    newInvoice.Vat = double.Parse(txtVAT.Text);
                    newInvoice.TotalCost = double.Parse(txtPriceSale.Text);

                    await invoiceService.PostInvoicesSellAsync(newInvoice);

                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.LoadUC(new UCInvoices());
                    }
                }
                else if (grdContractRentStartForm.Visibility == Visibility.Visible)
                {
                    dynamic selectedContract = cmbContractStart.SelectedItem;
                    newInvoice.ContractId = selectedContract.Contract.Id;
                    newInvoice.PaymentMethod = cmbPaymenthStart.Text.ToString();
                    newInvoice.Vat = double.Parse(txtVATStart.Text);

                    await invoiceService.PostInvoicesRentStartAsync(newInvoice);

                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.LoadUC(new UCInvoices());
                    }
                }
                else if (grdContractRentEndForm.Visibility == Visibility.Visible)
                {
                    dynamic selectedContract = cmbContractEnd.SelectedItem;
                    newInvoice.ContractId = selectedContract.Contract.Id;
                    newInvoice.PaymentMethod = cmbPaymenthEnd.Text.ToString();
                    newInvoice.TotalCost = double.Parse(txtVATEnd.Text);
                    newInvoice.Mileage = int.Parse(txtMilageEnd.Text);
                    newInvoice.Vat = double.Parse(txtVATStart.Text);

                    var penaltyIds = penalties.Select(p => p.Id).ToList();
                    await invoiceService.PostInvoicesRentEndAsync(newInvoice, penaltyIds);

                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.LoadUC(new UCInvoices());
                    }

                }

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom čuvanja računa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetForm()
        {
            cmbContractSale.SelectedItem = null;
            cmbPaymenthSale.SelectedItem = null;
            txtVAT.Text = string.Empty;
            txtPriceSale.Text = string.Empty;

            cmbContractStart.SelectedItem = null;
            cmbPaymenthStart.SelectedItem = null;
            txtVATStart.Text = string.Empty;

            cmbContractEnd.SelectedItem = null;
            cmbPaymenthEnd.SelectedItem = null;
            txtVATEnd.Text = string.Empty;
            txtMilageEnd.Text = string.Empty;

            penalties.Clear();
            lstSelectedPenalties.ItemsSource = null;



        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.LoadUC(new UCInvoices());
            }
        }
    }
}
