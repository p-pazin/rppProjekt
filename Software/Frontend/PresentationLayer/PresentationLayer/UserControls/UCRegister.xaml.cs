using System;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network.Dto;
using ServiceLayer.Network;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    public partial class UCRegister : UserControl
    {
        private readonly AuthService _apiService;

        public UCRegister()
        {
            InitializeComponent();
            var httpClient = NetworkService.GetHttpClient(new TokenManager());
            _apiService = new AuthService();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var newCompany = new NewCompanyDto
            {
                Name = CompanyNameTextBox.Text,
                City = CityTextBox.Text,
                Address = AddressTextBox.Text,
                Pin = PinTextBox.Text,
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Email = EmailTextBox.Text,
                Password = PasswordBox.Password
            };

            if (string.IsNullOrWhiteSpace(newCompany.Name) ||
                string.IsNullOrWhiteSpace(newCompany.Email) ||
                string.IsNullOrWhiteSpace(newCompany.Password))
            {
                MessageBox.Show("Molimo unesite sve podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _apiService.RegisterAsync(newCompany);
                MessageBox.Show("Registracija uspješna! Možete se sada prijaviti.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom registracije: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is LoginWindow lw)
            {
                lw.LoadUC(new UCLogin(lw));
            }
        }
    }
}
