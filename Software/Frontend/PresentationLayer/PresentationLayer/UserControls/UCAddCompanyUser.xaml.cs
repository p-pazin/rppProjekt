using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCAddCompanyUser.xaml
    /// </summary>
    public partial class UCAddCompanyUser : UserControl
    {
        private readonly CompanyService _companyService;

        public UCAddCompanyUser()
        {
            InitializeComponent();
            _companyService = new CompanyService();
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Molimo popunite sva polja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new RegisterUserDto
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password
            };

            try
            {
                await _companyService.PostCompanyUser(newUser);
                MessageBox.Show("Zaposlenik uspješno dodan!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dodavanja zaposlenika: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.LoadUC(new UCCompanyUsers());
        }
    }
}
