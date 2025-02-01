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
    /// Interaction logic for UCInvoices.xaml
    /// </summary>
    public partial class UCInvoices : UserControl
    {
        private readonly InvoiceService _invoiceService;

        public UCInvoices()
        {
            InitializeComponent();
            selectedWarning.Visibility = Visibility.Hidden;
            _invoiceService = new InvoiceService();
        }

        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrintInvoice_Click(object sender, RoutedEventArgs e)
        {
            var selectedInvoice = dgvInvoices.SelectedItem as InvoiceDto;

            if (selectedInvoice != null)
            {
                
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
