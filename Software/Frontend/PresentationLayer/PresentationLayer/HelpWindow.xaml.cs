using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            LoadHelpDocumentation();
        }

        private void LoadHelpDocumentation()
        {
            string helpFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help", "index.html");
            HelpBrowser.Navigate(new Uri(helpFilePath));
        }

        private void HelpBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadingIndicator.Visibility = Visibility.Collapsed;

        }
    }
}
