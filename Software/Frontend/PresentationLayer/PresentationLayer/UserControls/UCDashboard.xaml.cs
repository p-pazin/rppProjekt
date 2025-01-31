using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    public partial class UCDashboard : UserControl
    {
        private readonly UserService _userService;

        private readonly CompanyService _companyService;

        public UCDashboard()
        {
            InitializeComponent();
            _userService = new UserService();
            _companyService = new CompanyService();
            LoadUserData();
        }

        private async void LoadUserData()
        {
            try
            {
                UserDto user = await _userService.GetCurrentUserAsync();
                CompanyDto company = await _companyService.GetCurrentCompanyAsync();

                DataContext = new { user, company };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
