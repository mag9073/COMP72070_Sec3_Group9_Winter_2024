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

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminHomeView.xaml
    /// </summary>
    public partial class AdminHomeView : Window
    {
        public AdminHomeView()
        {
            InitializeComponent();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            AdminHomeView adminHomeView = new AdminHomeView();

            adminHomeView.Show();

            this.Close();
        }

        private void LogoutRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }
    }
}
