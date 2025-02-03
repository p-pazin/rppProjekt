using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarchiveAPI.Dto;
using ServiceLayer.Network.Dto;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCReservations.xaml
    /// </summary>
    public partial class UCReservations : UserControl
    {
        private readonly ReservationService _reservationService;

        public UCReservations()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
            LoadReservations();
            selectedWarning.Visibility = Visibility.Hidden;
            deletionWarning.Visibility = Visibility.Hidden;
        }

        private async void LoadReservations()
        {
            try
            {
                List<ReservationDto> reservations = await _reservationService.GetReservationsAsync();
                dgvReservations.ItemsSource = reservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja rezervacija: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddReservation_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddReservation());
                mw.AdjustUserControlMargin();
            }
        }

        private void btnEditReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dgvReservations.SelectedItem is ReservationDto selectedReservation)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCEditReservation(selectedReservation));
                    mw.AdjustUserControlMargin();
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }


        private async void btnDeleteReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dgvReservations.SelectedItem == null)
            {
                selectedWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => selectedWarning.Visibility = Visibility.Hidden);
                });
                return;
            }
            if (dgvReservations.SelectedItem is ReservationDto selectedReservation)
            {
                var messageBox = new DeletionWarningWindow("rezervaciju");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        await _reservationService.DeleteReservationsAsync(selectedReservation.Id);
                        LoadReservations();
                    }
                    catch (Exception)
                    {
                        deletionWarning.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
