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
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for DeletionWarningWindow.xaml
    /// </summary>
    public partial class DeletionWarningWindow : Window
    {
        public DeletionWarningWindow(string deletionObject)
        {
            InitializeComponent();
            txtMessage.Text = $"Jeste li sigurni da želite obrisati {deletionObject}?";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
