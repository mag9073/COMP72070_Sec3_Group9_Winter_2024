﻿using LogiPark.MVVM.Model;
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
            client.SendAllParkDataRequest();

            // receive back the response from the server which contains array of park data obj
            ParkDataManager.ParkData[] parks = client.ReceiveAllParkDataResponse();
            //var images = client.RequestAndReceiveImages();

            // Send for all park reviews data
            client.SendAllReviewsRequest();

            // Send for all park images 
            client.SendAllParkImagesRequest();

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
                if(reviewsForPark.Any())
                {
                    float totalRating = 0;

                    // Iterate through each park review and sum up each rating
                    for(int i = 0; i < reviewsForPark.Count; i++)
                    {
                        totalRating += reviewsForPark[i].Rating;
                    }

                    // Calculate for average rating
                    averageRating = totalRating / reviewsForPark.Count;
                }

                //var image = images.FirstOrDefault(img => img.FileName == park.GetParkName() + ".jpg"); // Assuming naming convention
                return new
                {
                    Name = park.GetParkName(),
                    Address = park.GetParkAddress(),
                    // Set park reviews calcualate average
                    AverageRating = averageRating,
                    //ImagePath ?
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
            FrameworkElement button = sender as FrameworkElement;
            if (button != null)
            {
                object park = button.DataContext;
                if (park != null)
                {
                    // TextBlock -> Text={Binding Name}
                    string parkName = park.GetType().GetProperty("Name")?.GetValue(park, null)?.ToString();
                    if (!string.IsNullOrEmpty(parkName))
                    {
                        ParkViewPage parkViewPage = new ParkViewPage(parkName);
                        Window window = new Window
                        {
                            Content = parkViewPage,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        };
                        window.Show();  // Right now, im creating this 
                    }
                }
            }
        }
    }
}
