using LogiPark.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ParkViewPage.xaml
    /// </summary>
    public partial class ParkViewPage : UserControl
    {
        private string _parkName;
        public ParkViewPage()
        {
        }

        public ParkViewPage(string parkName)
        {
            InitializeComponent();
            _parkName = parkName;   // Now we have the park name -> can request park data from server (data + reviews)
            ParkNameTextBlock.Text = parkName;  

        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"park name: { _parkName}");
        }
    }
}
