using System;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCNewPassword.xaml
    /// </summary>
    public partial class UCNewPassword : UserControl
    {
        private readonly AuthService _authService = new AuthService();
        private readonly string _userEmail;

        public UCNewPassword(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = txtPassword.Password;
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtNewPasswordAgain.Password;

            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Sva polja moraju biti ispunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Nova lozinka i potvrda lozinke se ne podudaraju!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                NewPasswordDto newPasswordDto = new NewPasswordDto
                {
                    Email = _userEmail,
                    Password = oldPassword,
                    NewPassword = newPassword
                };

                bool isSuccess = await _authService.ChangePasswordAsync(newPasswordDto);

                if (isSuccess)
                {
                    MessageBox.Show("Lozinka uspješno promijenjena!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Neuspjela promjena lozinke. Provjerite unesene podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do pogreške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
