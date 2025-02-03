using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using PresentationLayer.helpers;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCInvoices.xaml
    /// </summary>
    public partial class UCInvoices : UserControl
    {
        private readonly InvoiceService _invoiceService;
        private readonly ContractService _contractService;
        private readonly PdfHelper _pdfHelper;
        public UCInvoices()
        {
            InitializeComponent();
            selectedWarning.Visibility = Visibility.Hidden;
            _invoiceService = new InvoiceService();
            _contractService = new ContractService();
            _pdfHelper = new PdfHelper();
        }

        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddInvoice());
            }
        }

        private async void btnPrintInvoice_Click(object sender, RoutedEventArgs e)
        {
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedInvoice = dgvInvoices.SelectedItem as InvoiceDto;

            if (selectedInvoice != null)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    var linkedContract = await _contractService.GetContractAsync(selectedInvoice.ContractId);

                    if(linkedContract.Type == 1)
                    {
                        var contractDetails = await _contractService.GetContractSaleAsync(linkedContract.Id);
                        var generatedDocument = _pdfHelper.GenerateSaleInvoicePdf(contractDetails, selectedInvoice);

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "PDF Files|*.pdf",
                            FileName = $"Prodajni račun {linkedContract.ContactName} {selectedInvoice.DateOfCreation}.pdf"
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            File.WriteAllBytes(saveFileDialog.FileName, generatedDocument);
                        }
                    }
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInvoiceData();
        }

        public async void LoadInvoiceData()
        {
            try
            {
                List<InvoiceDto> invoices = await _invoiceService.GetInvoicesAsync();
                dgvInvoices.ItemsSource = invoices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
