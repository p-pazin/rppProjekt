using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
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

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCOfferCatalog.xaml
    /// </summary>
    public partial class UCOfferCatalog : UserControl
    {
        private OfferServices offerServices = new OfferServices();
        private ContactService contactService = new ContactService();
        private VehicleService vehicleService = new VehicleService();
        public UCOfferCatalog()
        {
            InitializeComponent();
            selectedWarning.Visibility = Visibility.Hidden;
            warning.Visibility = Visibility.Hidden;
            LoadOffersAsync();
        }

        private async void LoadOffersAsync()
        {
            dgvOffers.ItemsSource = await offerServices.GetOffers();
        }

        private void btnAddOffer_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddOffer());
            }
        }

        private void btnEditOffer_Click(object sender, RoutedEventArgs e)
        {
            var selectedOffer = dgvOffers.SelectedItem as OfferDto;
            if (selectedOffer != null) {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCEditOffer(selectedOffer));
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteOffer_Click(object sender, RoutedEventArgs e)
        {
            selectedWarning.Visibility = Visibility.Hidden;
            var selectedOffer = dgvOffers.SelectedItem as OfferDto;
            if (selectedOffer != null)
            {
                var deletionWarningWindow = new DeletionWarningWindow("ponudu");
                if (deletionWarningWindow.ShowDialog() == true)
                {
                    try
                    {
                        offerServices.DeleteOffer(selectedOffer.Id);
                        LoadOffersAsync();
                    }
                    catch (Exception ex)
                    {
                        warning.Visibility = Visibility.Visible;
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
