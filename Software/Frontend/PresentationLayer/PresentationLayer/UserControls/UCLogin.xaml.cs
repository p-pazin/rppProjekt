using System;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    public partial class UCLogin : UserControl
    {
        private readonly AuthService _apiService;
        private readonly TokenManager _tokenManager;
        private readonly Window _parentWindow;

        public UCLogin(Window parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
            _tokenManager = new TokenManager();
            var httpClient = NetworkService.GetHttpClient(_tokenManager);
            _apiService = new AuthService(httpClient);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Unesite email i lozinku.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string token = await _apiService.LoginAsync(email, password);

                if (!string.IsNullOrEmpty(token))
                {
                    _tokenManager.SaveToken(token);
                    MainWindow main = new MainWindow();
                    main.Show();
                    _parentWindow.Close();
                }
                else
                {
                    MessageBox.Show("Prijava neuspješna. Provjerite podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is LoginWindow lw)
            {
                lw.LoadUC(new UCRegister());
            }
            
        }


    }
}
