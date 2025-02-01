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
            infoWarning.Visibility = Visibility.Hidden;
            addUserWarning.Visibility = Visibility.Hidden;
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                infoWarning.Visibility = Visibility.Visible;
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

                if (Application.Current.MainWindow is MainWindow mainWindow)
                    mainWindow.LoadUC(new UCCompanyUsers(1));
            }
            catch (Exception)
            {
                addUserWarning.Visibility = Visibility.Visible;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.LoadUC(new UCCompanyUsers());
        }
    }
}
