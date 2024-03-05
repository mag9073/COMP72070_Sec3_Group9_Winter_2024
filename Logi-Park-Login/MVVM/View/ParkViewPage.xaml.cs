using LogiPark.MVVM.Model;
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
        private ProgramClient _client;
        public ParkViewPage()
        {
        }

        public ParkViewPage(string parkName)
        {
            InitializeComponent();
            this._client = new ProgramClient();
            _parkName = parkName;   // Now we have the park name -> can request park data from server (data + reviews)
            ParkNameTextBlock.Text = _parkName;  
            // 1. Request Park Reviews
            _client.SendParkReviewsRequest(_parkName);
            // 2. Request Park Data

            // 3. Request Park Image

            // Fetch and display the reviews
            ReceiveParkReviewsFromServer();
            DisplayParkImage(parkName);
        }

        private void ReceiveParkReviewsFromServer()
        {
            List<ParkReviewManager.ParkReviewData> reviews = _client.ReceiveParkReviewsResponse();

            for (int i = 0; i < reviews.Count; i++)
            {
                Console.WriteLine($"{reviews[i].UserName}: {reviews[i].Review} : {reviews[i].Rating} : {reviews[i].DateOfPosting}");
            }
        }

        private void DisplayParkImage(string parkName)
        {
            this.Dispatcher.Invoke(async () =>
            {
                _client.SendOneParkImageRequest(parkName);
                BitmapImage parkImage = _client.ReceiveOneParkImageResponse();

                SetParkImage(parkImage);
            });
        }

        public void SetParkImage(BitmapImage image)
        {
            ParkImage.Source = image;
        }


        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"park name: { _parkName}");
        }
    }
}
