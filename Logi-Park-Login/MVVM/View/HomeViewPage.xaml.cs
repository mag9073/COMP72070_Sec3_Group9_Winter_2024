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
    /// Interaction logic for HomeViewPage.xaml
    /// </summary>
    public partial class HomeViewPage : UserControl
    {
        public HomeViewPage()
        {
            InitializeComponent();
        }

        private void OnParkImageClick(object sender, RoutedEventArgs e)
        {
            string parkName = ((FrameworkElement)sender).Tag.ToString();

            ParkView parkView = new ParkView(parkName);
            parkView.Show();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
