using System;
using System.Windows;
using System.Windows.Controls;
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
            LoginContentFrame.Content = new UCLogin(this);
        }

        public void LoadUC(UserControl uc)
        {
            LoginContentFrame.Content = uc;
        }
    }
}
