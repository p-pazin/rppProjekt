using System;
using System.Threading.Tasks;
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
            infoWarning.Visibility = Visibility.Hidden;
            passwordWarning.Visibility = Visibility.Hidden;
            inputWarning.Visibility = Visibility.Hidden;
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
                inputWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => inputWarning.Visibility = Visibility.Hidden);
                });
                return;
            }

            if (newPassword != confirmPassword)
            {
                passwordWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => passwordWarning.Visibility = Visibility.Hidden);
                });
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
                    if (Application.Current.MainWindow is MainWindow mw)
                    {
                        mw.LoadUC(new UCDashboard(1));
                        mw.AdjustUserControlMargin();
                    }
                }
                else
                {
                    infoWarning.Visibility = Visibility.Visible;
                    Task.Delay(3000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() => infoWarning.Visibility = Visibility.Hidden);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do pogreške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCDashboard());
                mw.AdjustUserControlMargin();
            }
        }
    }
}
