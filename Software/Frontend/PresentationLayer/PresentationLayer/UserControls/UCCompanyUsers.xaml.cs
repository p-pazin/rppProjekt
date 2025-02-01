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
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCCompanyUsers.xaml
    /// </summary>
    public partial class UCCompanyUsers : UserControl
    {
        CompanyService companyService = new CompanyService();
        public UCCompanyUsers()
        {
            InitializeComponent();
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            try
            {
                List<UserDto> users = await companyService.GetCurrentCompanyUsersAsync();
                dgvUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddCompanyUser());
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItem is UserDto user)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                   // mw.LoadUC(new UCEditCompanyUser(user));
                }
            }
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItem is UserDto user)
            {
                try
                {
                    //await companyService.DeleteCompanyUserAsync(user.Id);
                    LoadUsersAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri brisanju korisnika: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCDashboard());
            }
        }
    }
}
