using PresentationLayer.enums;
using ServiceLayer.Network.Dto;
using ServiceLayer.Services;
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
using static PresentationLayer.enums.EnumModels;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCAddVehicle.xaml
    /// </summary>
    public partial class UCAddVehicle : UserControl
    {
        private VehicleService vehicleService = new VehicleService();
        public UCAddVehicle()
        {
            InitializeComponent();
            addVehicleWarning.Visibility = Visibility.Hidden;
            infoWarning.Visibility = Visibility.Hidden;
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.AdjustUserControlMargin();
            }
            LoadDropDowns();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCVehicleCatalog());
            }
        }

        private void LoadDropDowns()
        {
            cmbBrand.ItemsSource = Enum.GetValues(typeof(EnumBrands)).Cast<EnumBrands>()
                .Select(c => GetEnumDescription(c))
                .ToList();
            cmbBrand.SelectedIndex = 0;

            var selectedBrand = cmbBrand.SelectedItem.ToString();
            LoadModelsForBrand(selectedBrand);
        }

        private void LoadModelsForBrand(string brand)
        {
            var models = new List<string>();

            switch (brand)
            {
                case "BMW":
                    models = Enum.GetValues(typeof(BMWModel)).Cast<BMWModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Audi":
                    models = Enum.GetValues(typeof(AudiModel)).Cast<AudiModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Mercedes":
                    models = Enum.GetValues(typeof(MercedesModel)).Cast<MercedesModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Škoda":
                    models = Enum.GetValues(typeof(SkodaModel)).Cast<SkodaModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Volkswagen":
                    models = Enum.GetValues(typeof(VolkswagenModel)).Cast<VolkswagenModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Opel":
                    models = Enum.GetValues(typeof(OpelModel)).Cast<OpelModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Ford":
                    models = Enum.GetValues(typeof(FordModel)).Cast<FordModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Peugeot":
                    models = Enum.GetValues(typeof(PeugeotModel)).Cast<PeugeotModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Honda":
                    models = Enum.GetValues(typeof(HondaModel)).Cast<HondaModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Toyota":
                    models = Enum.GetValues(typeof(ToyotaModel)).Cast<ToyotaModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Fiat":
                    models = Enum.GetValues(typeof(FiatModel)).Cast<FiatModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Citroën":
                    models = Enum.GetValues(typeof(CitroenModel)).Cast<CitroenModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Ferrari":
                    models = Enum.GetValues(typeof(FerrariModel)).Cast<FerrariModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Lamborghini":
                    models = Enum.GetValues(typeof(LamborghiniModel)).Cast<LamborghiniModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Porsche":
                    models = Enum.GetValues(typeof(PorscheModel)).Cast<PorscheModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "AstonMartin":
                    models = Enum.GetValues(typeof(AstonMartinModel)).Cast<AstonMartinModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "Jaguar":
                    models = Enum.GetValues(typeof(JaguarModel)).Cast<JaguarModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                case "LandRover":
                    models = Enum.GetValues(typeof(LandRoverModel)).Cast<LandRoverModel>()
                        .Select(m => GetEnumDescription(m))
                        .ToList();
                    break;

                default:
                    models.Add("No models available");
                    break;
            }

            cmbModel.ItemsSource = models;
            cmbModel.SelectedIndex = 0;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute != null ? attribute.Description : value.ToString();
        }

        private void cmbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBrand = cmbBrand.SelectedItem.ToString();
            LoadModelsForBrand(selectedBrand);
        }

        private async void btnSaveVehicle_Click(object sender, RoutedEventArgs e)
        {
            infoWarning.Visibility = Visibility.Hidden;
            addVehicleWarning.Visibility = Visibility.Hidden;
            if(ValidateInputs() == false)
            {
                infoWarning.Visibility = Visibility.Visible;
                return;
            }
            var vehicle = new VehiclePost
            {
                Brand = cmbBrand.SelectedItem.ToString(),
                Model = cmbModel.SelectedItem.ToString(),
                Usage = cmbUsage.SelectedIndex + 1,
                ProductionYear = int.Parse(txtProductionYear.Text),
                Registration = txtRegistration.Text,
                Mileage = int.Parse(txtMileage.Text),
                Engine = txtEngine.Text,
                CubicCapacity = int.Parse(txtCubicCapacity.Text),
                EnginePower = int.Parse(txtEnginePower.Text),
                RegisteredTo = dtpRegisteredTo.SelectedDate.Value.ToString("yyyy-MM-dd"),
                Color = txtColor.Text,
                DriveType = txtDriveType.Text,
                Price = int.Parse(txtPrice.Text),
                TransmissionType = txtTransmissionType.Text,
                State = 1,
                Type = txtType.Text,
                Condition = txtCondition.Text,
                RentPrice = int.Parse(txtPrice.Text)
            };
            try
            {
                await vehicleService.PostVehicle(vehicle);
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCVehicleCatalog());
                }
            }
            catch (Exception ex)
            {
                addVehicleWarning.Visibility = Visibility.Visible;
            }
        }

        private bool ValidateInputs()
        {
            if (cmbBrand.SelectedIndex == -1 || cmbModel.SelectedIndex == -1 || cmbUsage.SelectedIndex == -1 || txtProductionYear.Text.Length == 0
                || txtRegistration.Text.Length == 0 || txtMileage.Text.Length == 0 || txtEngine.Text.Length == 0 || txtCubicCapacity.Text.Length == 0
                || txtEnginePower.Text.Length == 0 || dtpRegisteredTo.SelectedDate == null || txtColor.Text.Length == 0 || txtDriveType.Text.Length == 0
                || txtPrice.Text.Length == 0 || txtTransmissionType.Text.Length == 0 || txtType.Text.Length == 0 || txtCondition.Text.Length == 0
                || txtPrice.Text.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}
