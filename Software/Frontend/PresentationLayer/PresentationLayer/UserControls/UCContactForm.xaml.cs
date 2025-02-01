using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using PresentationLayer.enums;
using ServiceLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCContactForm.xaml
    /// </summary>
    public partial class UCContactForm : UserControl
    {
        private readonly ContactService _contactService;
        private ContactDto _contact;
        public UCContactForm(ContactDto contact = null)
        {
            InitializeComponent();
            var countries = Enum.GetValues(typeof(EnumCountries))
            .Cast<EnumCountries>()
            .Select(c => GetEnumDescription(c))
            .ToList();

            var cities = Enum.GetValues(typeof(EnumCities))
            .Cast<EnumCities>()
            .Select(c => GetEnumDescription(c))
            .ToList();

            var states = new List<string> { "Aktivan kontakt", "Neaktivan kontakt"};
            cmbCountry.ItemsSource = countries;
            cmbCity.ItemsSource = cities;
            cmbStatus.ItemsSource = states;
            _contactService = new ContactService();
            _contact = contact;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCountry.SelectedIndex = 0;
            cmbCity.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            if(_contact != null) {
                txtTitle.Text = "Ažuriranje kontakta";
                txtFirstName.Text = _contact.FirstName;
                txtLastName.Text = _contact.LastName;
                txtEmail.Text = _contact.Email;
                txtMobileNumber.Text = _contact.MobileNumber;
                txtAddress.Text = _contact.Address;
                txtPhoneNumber.Text = _contact.TelephoneNumber;
                txtOIB.Text = _contact.Pin;
                txtDescription.Text = _contact.Description;
                cmbCountry.SelectedItem = _contact.Country;
                cmbCity.SelectedItem = _contact.City;
                cmbStatus.SelectedItem = _contact.State == 1 ? "Aktivan kontakt" : "Neaktivan kontakt";
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool inputsValid = ValidateInputs();

            if(inputsValid) {
                DateTime currentDate = DateTime.Now;
                var newContact = new ContactDto
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Pin = txtOIB.Text,
                    Email = txtEmail.Text,
                    Description = txtDescription.Text,
                    TelephoneNumber = txtPhoneNumber.Text,
                    MobileNumber = txtMobileNumber.Text,
                    Address = txtAddress.Text,
                    Country = cmbCountry.SelectedItem as string,
                    City = cmbCity.SelectedItem as string,
                    State = (cmbStatus.SelectedItem as string == "Aktivan kontakt") ? 1 : 0,
                    DateOfCreation = currentDate.ToString("yyyy-MM-dd"),
                    Id = _contact?.Id ?? 0,
                };

                try
                {
                    if (_contact != null)
                    {
                        _contactService.PutContactAsync(newContact);
                    }
                    else
                    {
                        _contactService.PostContactAsync(newContact);
                    }
                    if(Application.Current.MainWindow is MainWindow mw)
                    {
                        var ucContacts = new UCContacts();
                        mw.LoadUC(ucContacts);
                        mw.AdjustUserControlMargin();
                        Application.Current.Dispatcher.InvokeAsync(async () =>
                        {
                            await Task.Delay(100);
                            ucContacts.LoadContactsData();
                        });
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Greška pri dodavanju kontakta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Potrebno je popuniti sva polja!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if(txtFirstName.Text.Length == 0 || txtLastName.Text.Length == 0 || txtOIB.Text.Length == 0 || txtEmail.Text.Length == 0
                || txtDescription.Text.Length == 0 || txtPhoneNumber.Text.Length == 0 || txtMobileNumber.Text.Length == 0 ||
                txtAddress.Text.Length == 0) 
            {
               return false;
            }
            return true;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
