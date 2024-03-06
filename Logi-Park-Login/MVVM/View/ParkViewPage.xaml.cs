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

            // 1. Receive Park Reviews
            DisplayParkReviews();

            // 2. Request Park Data
            _client.SendOneParkDataRequest(_parkName);

            // 2. Receive Park Data
            DisplayParkData();

            // 3. Request Park Image
            _client.SendOneParkImageRequest(_parkName);

            // 3. Request Park Image
            DisplayParkImage(_parkName);
        }

        private void DisplayParkReviews ()
        {
            List<ParkReviewManager.ParkReviewData> reviews = _client.ReceiveParkReviewsResponse();

            for (int i = 0; i < reviews.Count; i++)
            {
                Console.WriteLine($"{reviews[i].UserName}: {reviews[i].Review} : {reviews[i].Rating} : {reviews[i].DateOfPosting}");
            }
        }

        private void DisplayParkData()
        {
            this.Dispatcher.Invoke(async () =>
            {
                ParkDataManager.ParkData parkData = _client.ReceiveOneParkDataResponse();
                ParkNameTextBlock.Text = parkData.parkName;

                ParkRatingTextBlock.Text = $"Rating: {parkData.parkReview} stars"; 
                ParkReviewsCountTextBlock.Text = $"{parkData.numberOfReviews} reviews";
                ParkAddressTextBlock.Text = parkData.parkAddress;

                // Dynamically generate star ratings based on the rounded-down rating
                var roundedRating = Math.Floor(parkData.parkReview);
                var stars = new List<BitmapImage>();
                for (int i = 0; i < roundedRating; i++)
                {
                    stars.Add(new BitmapImage(new Uri("/Assets/Icons/star-rating-icon.png", UriKind.Relative)));
                }
                StarRatingControl.ItemsSource = stars;

            });
        }

        private void DisplayParkImage(string parkName)
        {
            this.Dispatcher.Invoke(async () =>
            {
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
