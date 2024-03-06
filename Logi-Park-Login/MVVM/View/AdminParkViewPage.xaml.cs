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

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminParkViewPage.xaml
    /// </summary>
    public partial class AdminParkViewPage : UserControl
    {
        public AdminParkViewPage()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ParkEditViewPage parkEditView = new ParkEditViewPage();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();

            Window window = new Window
            {
                Content = parkEditView,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Show();
        }

        private void DeleteReviewButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteParkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
