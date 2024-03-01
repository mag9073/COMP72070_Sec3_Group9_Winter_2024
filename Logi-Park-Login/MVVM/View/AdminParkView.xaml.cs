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
    /// Interaction logic for AdminParkView.xaml
    /// </summary>
    public partial class AdminParkView : Window
    {
        public AdminParkView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            AdminHomeView adminHomeView = new AdminHomeView();

            adminHomeView.Show();

            this.Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            AdminHomeView homeView = new AdminHomeView();

            homeView.Show();

            this.Close();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }
    }
}
