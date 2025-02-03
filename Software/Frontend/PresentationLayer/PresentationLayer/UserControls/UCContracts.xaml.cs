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
    /// Interaction logic for UCContracts.xaml
    /// </summary>
    public partial class UCContracts : UserControl
    {
        private readonly ContractService _contractService;
        private readonly PdfHelper _pdfHelper;

        public UCContracts()
        {
            InitializeComponent();
            _contractService = new ContractService();
            deletionWarning.Visibility = Visibility.Hidden;
            signedWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            _pdfHelper = new PdfHelper();
        }

        private void btnAddContract_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCContractForm());
                mw.AdjustUserControlMargin();
            }
        }

        private void btnEditContract_Click(object sender, RoutedEventArgs e)
        {
            var selectedContract = dgvContracts.SelectedItem as ContractDto;

            if (selectedContract != null)
            {
                if(selectedContract.Signed == 0)
                {
                    if (Application.Current.MainWindow is MainWindow mw)
                    {
                        if(selectedContract.Type == 1)
                        {
                            mw.LoadUC(new UCEditContractSale(selectedContract.Id));
                            mw.AdjustUserControlMargin();

                        }
                        else
                        {
                            mw.LoadUC(new UCEditContractRent(selectedContract.Id));
                            mw.AdjustUserControlMargin();
                        }
                    }
                    
                }
                else
                {
                    selectedWarning.Visibility = Visibility.Hidden;
                    signedWarning.Visibility = Visibility.Visible;
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedContract = dgvContracts.SelectedItem as ContractDto;

            if (selectedContract != null)
            {
                var messageBox = new DeletionWarningWindow("ugovor");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        _contractService.DeleteContractAsync(selectedContract.Id);
                        LoadContractsData();
                    }
                    catch (Exception ex)
                    {
                        deletionWarning.Visibility = Visibility.Visible;
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
            LoadContractsData();
        }

        public async void LoadContractsData()
        {
            try
            {
                List<ContractDto> contracts = await _contractService.GetContractsAsync();
                dgvContracts.ItemsSource = contracts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnPrintContract_Click(object sender, RoutedEventArgs e)
        {
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedContract = dgvContracts.SelectedItem as ContractDto;

            if (selectedContract != null)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    if (selectedContract.Type == 1)
                    {
                        var contractDetails = await _contractService.GetContractSaleAsync(selectedContract.Id);
                        var generatedDocument = _pdfHelper.GenerateSaleContractPdf(contractDetails);

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "PDF Files|*.pdf",
                            FileName = $"Kupoprodajni ugovor {selectedContract.ContactName} {selectedContract.DateOfCreation}.pdf"
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            File.WriteAllBytes(saveFileDialog.FileName, generatedDocument);
                        }

                        /*mw.LoadUC(new UCDocumentPreview(generatedDocument));
                        mw.AdjustUserControlMargin();*/
                    }
                    else
                    {
                        var contractDetails = await _contractService.GetContractRentAsync(selectedContract.Id);
                        var generatedDocument = _pdfHelper.GenerateRentContractPdf(contractDetails);

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "PDF Files|*.pdf",
                            FileName = $"Najmodavni ugovor {selectedContract.ContactName} {selectedContract.DateOfCreation}.pdf"
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
    }
}
