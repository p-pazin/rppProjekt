using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PresentationLayer.UserControls;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;

            var tokenManager = new TokenManager();
            string token = tokenManager.GetToken();
            LoginContentFrame.Content = new UCLogin(this);
            if (!string.IsNullOrEmpty(token))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

        }

        public void LoadUC(UserControl uc)
        {
            LoginContentFrame.Content = uc;
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                ShowHelp();
            }
        }

        private void ShowHelp()
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }
    }
}
