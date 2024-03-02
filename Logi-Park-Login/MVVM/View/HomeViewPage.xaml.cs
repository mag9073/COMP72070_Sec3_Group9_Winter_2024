using LogiPark.MVVM.Model;
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
        private ProgramClient client;
        public HomeViewPage()
        {
            this.client = new ProgramClient();
            InitializeComponent();
            this.RequestAllParkData();
        }

        private void RequestAllParkData()
        {
            // send request to the server retrieve all the park data info - we send empty body with the header containing flag to get all park data
            client.SendParkDataAllRequest();

            // receive back the response from the server which contains array of park data obj
            ParkDataManager.ParkData[] parks = client.ReceiveParkDataAllResponse();

            // make sure that we use UI thread - thread safe
            Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < parks.Length; i++)
                {
                    Console.WriteLine($"{parks[i].GetParkName()} {parks[i].GetParkAddress()} - {parks[i].GetParkReview()} stars -");
                }
            });
        }

        private void OnParkImageClick(object sender, RoutedEventArgs e)
        {
            ParkView parkView = new ParkView();
            parkView.Show();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
