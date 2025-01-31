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
    /// Interaction logic for UCDashboard.xaml
    /// </summary>
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
