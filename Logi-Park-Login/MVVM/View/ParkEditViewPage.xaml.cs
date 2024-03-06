using LogiPark.MVVM.ViewModel;
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
    /// Interaction logic for ParkEditViewPage.xaml
    /// </summary>
    public partial class ParkEditViewPage : UserControl
    {
        public ParkEditViewPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AdminParkViewPage parkEditView = new AdminParkViewPage();

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

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
