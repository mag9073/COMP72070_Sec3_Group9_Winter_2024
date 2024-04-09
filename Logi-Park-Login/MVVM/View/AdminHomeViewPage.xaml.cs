using LogiPark.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminHomeViewPage.xaml
    /// </summary>
    public partial class AdminHomeViewPage : UserControl
    {
        private ProgramClient client;

        public AdminHomeViewPage()
        {
            this.client = new ProgramClient();
            InitializeComponent();
            this.RequestAllParkData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AdminAddParkViewPage parkView = new AdminAddParkViewPage();

            Window window = new Window
            {
                Content = parkView,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();  // Right now, im creating this
        }

        private void RequestAllParkData()
        {
            // send request to the server retrieve all the park data info - we send empty body with the header containing flag to get all park data
            client.SendAllParkDataRequest();

            // receive back the response from the server which contains array of park data obj
            ParkDataManager.ParkData[] parks = client.ReceiveAllParkDataResponse();
            //var images = client.RequestAndReceiveImages();

            // Send for all park reviews data
            client.SendAllReviewsRequest();

            // Receive for all park reviews data
            List<ParkReviewManager.ParkReviewData> parkReviews = client.ReceiveParkReviewsResponse();

            // We make it annoynmous types which consists of name, address, review
            var parkCards = parks.Select(park =>
            {

                // Here similar to what we did in mySQL, we use where to filter down our results for matching park name and add it to the list 
                List<ParkReviewManager.ParkReviewData> reviewsForPark = parkReviews.Where(review => review.ParkName == park.GetParkName()).ToList();

                // Calculate average rating; if there are no reviews, default to 0
                float averageRating = 0;

                // As long there is any reviews from the park
                if (reviewsForPark.Any())
                {
                    float totalRating = 0;

                    // Iterate through each park review and sum up each rating
                    for (int i = 0; i < reviewsForPark.Count; i++)
                    {
                        totalRating += reviewsForPark[i].Rating;
                    }

                    // Calculate for average rating
                    averageRating = totalRating / reviewsForPark.Count;
                }

                string imagefile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"ParkImages/" + park.GetParkName() + ".jpg");
                if (!File.Exists(imagefile))
                {
                    client.SendOneParkImageRequest(park.GetParkName());
                    client.ReceiveOneParkImageResponseToFile(imagefile);
                }

                return new
                {
                    Name = park.GetParkName(),
                    Address = park.GetParkAddress(),
                    // Set park reviews calcualate average
                    AverageRating = averageRating,
                    ImagePath = imagefile,
                };
            }).ToList();   // Convert it back to list for the xaml card to dynamically rendered.

            // make sure that we use UI thread - thread safe -> we update our xaml park card
            Dispatcher.Invoke(() =>
            {
                ParksItemsControl.ItemsSource = parkCards;
            });
        }

        private void OnParkCardClick(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                var park = button.DataContext;
                if (park != null)
                {
                    // TextBlock -> Text={Binding Name}
                    string parkName = park.GetType().GetProperty("Name")?.GetValue(park, null)?.ToString();
                    if (!string.IsNullOrEmpty(parkName))
                    {
                        AdminParkViewPage parkViewPage = new AdminParkViewPage(parkName);
                        Window window = new Window
                        {
                            Content = parkViewPage,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            MaxHeight = 800
                        };
                        window.Show();  // Right now, im creating this 
                    }
                }
            }
        }
    }
}
