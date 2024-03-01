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

        //private bool admin;

        public HomeViewPage()
        {
            InitializeComponent();
        }

        //public HomeViewPage(bool Admin)
        //{
        //    InitializeComponent();
        //    this.admin = Admin;
        //}

        private void OnParkImageClick(object sender, RoutedEventArgs e)
        {
            ParkView parkView = new ParkView();
            parkView.Show();

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}
