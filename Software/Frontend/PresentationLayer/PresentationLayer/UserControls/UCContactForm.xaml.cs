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
using CarchiveAPI.Dto;
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
            var countries = new List<string>
            {
                "Hrvatska", "Afganistan", "Albanija", "Alžir", "Andora", "Angola", "Antigva i Barbuda", "Argentina", "Armenija",
                "Australija", "Austrija", "Azerbajdžan", "Bahami", "Bahrein", "Bangladeš", "Barbados", "Belgija", "Belize", "Benin",
                "Bjelorusija", "Bocvana", "Bolivija", "Bosna i Hercegovina", "Brazil", "Brunej", "Bugarska", "Burkina Faso", "Burundi",
                "Butan", "Cipar", "Crna Gora", "Čad", "Češka", "Čile", "Danska", "Dominika", "Dominikanska Republika", "Džibuti", "Egipat",
                "Ekvador", "Ekvatorska Gvineja", "Eritreja", "Estonija", "Eswatini", "Etiopija", "Fidži", "Filipini", "Finska", "Francuska",
                "Gabon", "Gambija", "Gana", "Grčka", "Grenada", "Gruzija", "Gvajana", "Gvatemala", "Gvineja", "Gvineja Bisau", "Haiti",
                "Honduras", "Indija", "Indonezija", "Irak", "Iran", "Irska", "Island", "Istočni Timor", "Italija", "Izrael", "Jamajka",
                "Japan", "Jemen", "Jordan", "Južna Afrika", "Južni Sudan", "Kambodža", "Kamerun", "Kanada", "Katar", "Kazahstan", "Kenija",
                "Kina", "Kirgistan", "Kiribati", "Kolumbija", "Komori", "Kongo", "Kongo (DR)", "Kosovo", "Kostarika", "Kuba", "Kuvajt",
                "Laos", "Latvija", "Lesoto", "Liban", "Liberija", "Libija", "Lihtenštajn", "Litva", "Luksemburg", "Madagaskar", "Mađarska",
                "Malavi", "Maldivi", "Malezija", "Mali", "Malta", "Maroko", "Maršalovi Otoci", "Mauricijus", "Mauritanija", "Meksiko",
                "Mikronezija", "Mjanmar", "Moldavija", "Monako", "Mongolija", "Mozambik", "Namibija", "Nauru", "Nepal", "Niger", "Nigerija",
                "Nikaragva", "Nizozemska", "Norveška", "Novi Zeland", "Njemačka", "Obala Bjelokosti", "Oman", "Pakistan", "Palau", "Panama",
                "Papua Nova Gvineja", "Paragvaj", "Peru", "Poljska", "Portugal", "Ruanda", "Rumunjska", "Rusija", "Salvador", "Samoa",
                "San Marino", "Saudijska Arabija", "Sjeverna Koreja", "Sjeverna Makedonija", "Sjedinjene Američke Države", "Singapur",
                "Sirija", "Sierra Leone", "Slovačka", "Slovenija", "Solomonski Otoci", "Somalija", "Srbija", "Srednjoafrička Republika",
                "Sudan", "Surinam", "Sveta Lucija", "Sveti Kristofor i Nevis", "Sveti Vincent i Grenadini", "Španjolska", "Šri Lanka",
                "Švedska", "Švicarska", "Tadžikistan", "Tajland", "Tanzanija", "Togo", "Tonga", "Trinidad i Tobago", "Tunis", "Turska",
                "Turkmenistan", "Tuvalu", "Uganda", "Ukrajina", "Ujedinjeni Arapski Emirati", "Ujedinjeno Kraljevstvo", "Urugvaj",
                "Uzbekistan", "Vanuatu", "Vatikan", "Venezuela", "Vijetnam", "Zambija", "Zelenortska Republika", "Zimbabve"
            };
            var cities = new List<string>
            {
                "Bakar", "Beli Manastir", "Belišće", "Benkovac", "Biograd na Moru", "Bjelovar", "Buje", "Buzet", "Cres", "Crikvenica",
                "Čabar", "Čakovec", "Čazma", "Daruvar", "Delnice", "Donja Stubica", "Donji Miholjac", "Drniš", "Dubrovnik", "Duga Resa",
                "Dugo Selo", "Đakovo", "Đurđevac", "Garešnica", "Glina", "Gospić", "Grubišno Polje", "Hrvatska Kostajnica", "Hvar",
                "Ilok", "Imotski", "Ivanec", "Ivanić-Grad", "Jastrebarsko", "Karlovac", "Kastav", "Kaštela", "Klanjec", "Knin", "Komiža",
                "Koprivnica", "Korčula", "Kraljevica", "Krapina", "Križevci", "Krk", "Kutina", "Kutjevo", "Labin", "Lepoglava", "Lipik",
                "Ludbreg", "Makarska", "Mali Lošinj", "Metković", "Mursko Središće", "Našice", "Nin", "Nova Gradiška", "Novalja", "Novi Marof",
                "Novi Vinodolski", "Novigrad", "Novska", "Obrovac", "Ogulin", "Omiš", "Opatija", "Opuzen", "Orahovica", "Oroslavje", "Osijek",
                "Otočac", "Otok", "Ozalj", "Pag", "Pakrac", "Pazin", "Petrinja", "Pleternica", "Ploče", "Popovača", "Poreč", "Požega",
                "Pregrada", "Prelog", "Pula", "Rab", "Rijeka", "Rovinj", "Samobor", "Senj", "Sinj", "Sisak", "Skradin", "Slatina",
                "Slavonski Brod", "Slunj", "Solin", "Split", "Stari Grad", "Supetar", "Sveta Nedelja", "Sveti Ivan Zelina", "Šibenik",
                "Trilj", "Trogir", "Umag", "Valpovo", "Varaždin", "Varaždinske Toplice", "Velika Gorica", "Vinkovci", "Virovitica", "Vis",
                "Vodice", "Vodnjan", "Vrbovec", "Vrbovsko", "Vrgorac", "Vrlika", "Vukovar", "Zabok", "Zadar", "Zagreb", "Zaprešić", "Zlatar",
                "Županja"
            };
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
            bool inputsValid = validateInputs();

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
                        mw.LoadUC(new UCContacts());
                        mw.AdjustUserControlMargin();
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

        private bool validateInputs()
        {
            if(txtFirstName.Text.Length == 0 || txtLastName.Text.Length == 0 || txtOIB.Text.Length == 0 || txtEmail.Text.Length == 0
                || txtDescription.Text.Length == 0 || txtPhoneNumber.Text.Length == 0 || txtMobileNumber.Text.Length == 0 ||
                txtAddress.Text.Length == 0) 
            {
               return false;
            }
            return true;
        }
    }
}
