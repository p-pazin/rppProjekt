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
using LiveCharts.Wpf;
using LiveCharts;
using ServiceLayer.Services;
using CarchiveAPI.Dto;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCStats.xaml
    /// </summary>
    public partial class UCStats : UserControl
    {
        private readonly StatsService _statsService;
        public UCStats()
        {
            InitializeComponent();
            _statsService = new StatsService();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var contactStatusStats = await _statsService.GetContactStatusStatsAsync();
            var contactCreationStats = await _statsService.GetContactCreationStatsAsync();
            var invoiceCreationStats = await _statsService.GetInvoiceCreationStatsAsync();

            txtPieChartContactStatusTitle.Visibility = Visibility.Visible;
            SetupPieChart(contactStatusStats);
            txtLineChartContactTitle.Visibility = Visibility.Visible;
            SetupLineChart(LineChartContactCreation, contactCreationStats);
            txtLineChartInvoiceTitle.Visibility = Visibility.Visible;
            SetupLineChart(LineChartInvoiceCreation, invoiceCreationStats);
        }
        private void SetupPieChart(ContactStatusStatsDto contactStatusStats)
        {
            PieChartStatus.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<double> { contactStatusStats.ActiveCount },
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#F65B43"),
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                    Title = "Aktivan"
                },
                new PieSeries
                {
                    Values = new ChartValues<double> { contactStatusStats.InactiveCount },
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#BC402D"),
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                    Title = "Neaktivan"
                }
            };
        }


        private void SetupLineChart(CartesianChart chart, YearlyInfoDto stats)
        {
            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = stats.Year.ToString(),
                    Values = new ChartValues<double>
                    {
                        stats.Jan, stats.Feb, stats.Mar, stats.Apr, stats.May, stats.Jun,
                        stats.Jul, stats.Aug, stats.Sep, stats.Oct, stats.Nov, stats.Dec
                    },
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#D44B36"), 
                    Fill = new SolidColorBrush(Color.FromArgb(102, 246, 91, 67)),
                    PointGeometrySize = 10
                }
            };

            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis
            {
                Title = "Mjesec",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                Separator = new LiveCharts.Wpf.Separator { Step = 1 }
            });

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                Title = "Broj",
                MinValue = 0
            });
        }
    }
}
