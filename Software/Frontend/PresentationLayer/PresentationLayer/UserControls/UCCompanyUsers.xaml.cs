using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCCompanyUsers.xaml
    /// </summary>
    public partial class UCCompanyUsers : UserControl
    {
        CompanyService companyService = new CompanyService();
        UserService userService = new UserService();
        public UCCompanyUsers(int info = 0)
        {
            InitializeComponent();
            LoadUsersAsync();
            selectedWarning.Visibility = Visibility.Hidden;
            deletionWarning.Visibility = Visibility.Hidden;
            addSuccess.Visibility = Visibility.Hidden;
            updateSuccess.Visibility = Visibility.Hidden;

            if (info == 1)
            {
                addSuccess.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => addSuccess.Visibility = Visibility.Hidden);
                });
            }
            if (info == 2)
            {
                updateSuccess.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => updateSuccess.Visibility = Visibility.Hidden);
                });
            }

        }

        private async void LoadUsersAsync()
        {
            try
            {
                List<UserDto> users = await companyService.GetCurrentCompanyUsersAsync();
                dgvUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddCompanyUser());
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItem == null)
            {
                selectedWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => selectedWarning.Visibility = Visibility.Hidden);
                });
                return;
            }
            if (dgvUsers.SelectedItem is UserDto user)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                   mw.LoadUC(new UCEditCompanyUser(user));
                }
            }
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgvUsers.SelectedItem == null)
            {
                selectedWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => selectedWarning.Visibility = Visibility.Hidden);
                });
                return;
            }
            if (dgvUsers.SelectedItem is UserDto user)
            {
                var messageBox = new DeletionWarningWindow("zaposlenika");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        await userService.DeleteCompanyUser(user.Id);
                        LoadUsersAsync();
                    }
                    catch (Exception)
                    {
                        deletionWarning.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCDashboard());
            }
        }

    }
}
