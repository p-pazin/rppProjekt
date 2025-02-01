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
    /// Interaction logic for UCEditOffer.xaml
    /// </summary>
    public partial class UCEditOffer : UserControl
    {
        private VehicleService vehicleService = new VehicleService();
        public UCEditOffer(OfferDto offer)
        {
            InitializeComponent();
            txtTitle.Text = offer.Title;
            txtPrice.Text = offer.Price.ToString();
            dtpDateOfCreation.SelectedDate = offer.DateOfCreation;
            LoadVehiclesDropDown();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCOfferCatalog());
            }
        }

        private async void LoadVehiclesDropDown()
        {
            cmbVehicles.ItemsSource = await vehicleService.GetVehiclesSale();
        }
    }
}
