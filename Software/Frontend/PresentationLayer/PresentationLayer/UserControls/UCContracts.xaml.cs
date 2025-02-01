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
    /// Interaction logic for UCContracts.xaml
    /// </summary>
    public partial class UCContracts : UserControl
    {
        private readonly ContractService _contractService;

        public UCContracts()
        {
            InitializeComponent();
            _contractService = new ContractService();
            deletionWarning.Visibility = Visibility.Hidden;
            signedWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
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
                var result = MessageBox.Show("Jeste li sigurni da želite obrisati ugovor?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
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
    }
}
