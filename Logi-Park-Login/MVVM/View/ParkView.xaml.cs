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
    /// Interaction logic for ParkView.xaml
    /// </summary>
    public partial class ParkView : Window
    {
        private string parkName;

        public ParkView(string parkName)
        {
            InitializeComponent();
            this.parkName = parkName;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            ClientHomeView adminHomeView = new ClientHomeView();

            adminHomeView.Show();

            this.Close();
        }

        private void HomeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            ClientHomeView homeView = new ClientHomeView();

            homeView.Show();

            this.Close();
        }

        private void MapRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Handle back to main menu label clicked
            MapView mapView = new MapView();

            mapView.Show();

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
