using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCEditCompanyUser.xaml
    /// </summary>
    public partial class UCEditCompanyUser : UserControl
    {
        private readonly UserService _userService;
        private UserDto _user;

        public UCEditCompanyUser(UserDto user)
        {
            InitializeComponent();
            _userService = new UserService();
            _user = user;

            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtEmail.Text = user.Email;

            infoWarning.Visibility = Visibility.Hidden;
            updateUserWarning.Visibility = Visibility.Hidden;
        }

        private async void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    infoWarning.Visibility = Visibility.Visible;
                    return;
                }

                _user.FirstName = txtFirstName.Text;
                _user.LastName = txtLastName.Text;
                _user.Email = txtEmail.Text;

                await UpdateCompanyUserAsync(_user);

                if (Application.Current.MainWindow is MainWindow mainWindow)
                    mainWindow.LoadUC(new UCCompanyUsers(2));
            }
            catch (Exception)
            {
                infoWarning.Visibility = Visibility.Visible;
            }
        }

        private async Task UpdateCompanyUserAsync(UserDto updatedUser)
        {
            await _userService.PutCompanyUser(updatedUser);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCCompanyUsers());
            }
        }
    }
}
