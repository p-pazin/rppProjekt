using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCDashboard.xaml
    /// </summary>
    public partial class UCDashboard : UserControl
    {
        private readonly UserService _userService;
        private readonly CompanyService _companyService;
        UserDto user = new UserDto();

        public UCDashboard(int info = 0)
        {
            InitializeComponent();
            _userService = new UserService();
            _companyService = new CompanyService();
            LoadUserData();
            passwordChangeSuccess.Visibility = Visibility.Hidden;
            JustFrame.Visibility = Visibility.Hidden;

            if (info == 1)
            {
                passwordChangeSuccess.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => passwordChangeSuccess.Visibility = Visibility.Hidden);
                });
            }
        }

        private async void LoadUserData()
        {
            try
            {
                user = await _userService.GetCurrentUserAsync();
                CompanyDto company = await _companyService.GetCurrentCompanyAsync();

                DataContext = new { user, company };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCNewPassword(user.Email));
                mw.AdjustUserControlMargin();
            }
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCCompanyUsers());
                mw.AdjustUserControlMargin();
            }
        }
    }
}
