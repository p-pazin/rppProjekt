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
    /// Interaction logic for UCAds.xaml
    /// </summary>
    public partial class UCAds : UserControl
    {
        private readonly AdService _adService;

        public UCAds()
        {
            InitializeComponent();
            _adService = new AdService();
            LoadAds();
            selectedWarning.Visibility = Visibility.Hidden;
            deletionWarning.Visibility = Visibility.Hidden;
        }

        private async void LoadAds()
        {
            try
            {
                List<AdDto> ads = await _adService.GetAdsAsync();
                dgvAds.ItemsSource = ads;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja oglasa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddAd_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LoadUC(new UCAddAd());
                mw.AdjustUserControlMargin();
            }
        }

        private void btnEditAd_Click(object sender, RoutedEventArgs e)
        {
            if (dgvAds.SelectedItem is AdDto selectedAd)
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.LoadUC(new UCEditAd(selectedAd));
                    mw.AdjustUserControlMargin();
                }
            }
            else
            {
                selectedWarning.Visibility = Visibility.Visible;
            }
        }

        private async void btnDeleteAd_Click(object sender, RoutedEventArgs e)
        {
            if (dgvAds.SelectedItem == null)
            {
                selectedWarning.Visibility = Visibility.Visible;
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => selectedWarning.Visibility = Visibility.Hidden);
                });
                return;
            }
            if (dgvAds.SelectedItem is AdDto selectedAd)
            {
                var messageBox = new DeletionWarningWindow("oglas");
                if (messageBox.ShowDialog() == true)
                {
                    try
                    {
                        await _adService.DeleteAdAsync(selectedAd.Id);
                        LoadAds();
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
