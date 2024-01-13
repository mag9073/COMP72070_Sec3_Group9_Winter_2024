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

namespace Logi_Park_Login.View
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Window
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            // For now will just tell user back to login page
            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }

        private void SignIn_Handler(object sender, RoutedEventArgs e)
        {
            // Handle sign in label clicked
             LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }
    }
}
