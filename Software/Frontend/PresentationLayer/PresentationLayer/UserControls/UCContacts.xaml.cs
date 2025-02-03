using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCContacts.xaml
    /// </summary>
    public partial class UCContacts : UserControl
    {
        private readonly ContactService _contactService;
        public UCContacts()
        {
            InitializeComponent();
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            _contactService = new ContactService();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContactsData();
        }

        public async void LoadContactsData()
        {
            try
            {
                List<ContactDto> contacts = await _contactService.GetContactsAsync();
                dgvContacts.ItemsSource = contacts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dohvaćanju podataka: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCContactForm());
                mw.AdjustUserControlMargin();
            }
        }

        private void btnEditContact_Click(object sender, RoutedEventArgs e)
        {
            var selectedContact = dgvContacts.SelectedItem as ContactDto;

            if(selectedContact != null) {
                if(Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCContactForm(selectedContact));
                    mw.AdjustUserControlMargin();
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            deletionWarning.Visibility = Visibility.Hidden;
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedContact = dgvContacts.SelectedItem as ContactDto;

            if(selectedContact != null)
            {
                var messageBox = new DeletionWarningWindow("kontakt");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        _contactService.DeleteContactAsync(selectedContact.Id);
                        LoadContactsData();
                    }
                    catch (Exception ex)
                    {
                        deletionWarning.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }
    }
}
