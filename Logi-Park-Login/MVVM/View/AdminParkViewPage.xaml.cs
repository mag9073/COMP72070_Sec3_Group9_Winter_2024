using LogiPark.MVVM.Model;
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
    /// 

    public partial class AdminParkViewPage : UserControl
    {
        private string _parkName;
        private ProgramClient _client;
        private float _averageRating = 0;
        private ParkDataManager.ParkData parkData;
        private BitmapImage _parkImage;

        public AdminParkViewPage()
        {

        }

        public AdminParkViewPage(string parkName)
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

        private void DisplayParkReviews()
        {
            // All the matching reviews are saved in a list because there could be more than one reviews per park
            List<ParkReviewManager.ParkReviewData> reviews = _client.ReceiveParkReviewsResponse();

            // In case there is any reviews stack panel already exist then we clear it first before starting to create reviews card
            ReviewsStackPanel.Children.Clear();

            // Store park review rating 
            float totalRating = 0;

            // Iterate through each review and build a review card
            foreach (ParkReviewManager.ParkReviewData review in reviews)
            {

                // get the total rating
                totalRating += review.Rating;

                // Main container for each review
                Border reviewCard = new Border
                {
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush(ColorConverter.ConvertFromString("#F0F0F0") as Color? ?? Colors.LightGray),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(5)
                };

                // Instantiate a StackPannel for the review content
                StackPanel reviewContent = new StackPanel();

                // Instantiate a StackPannel for the user info with properties inside
                StackPanel userInfoPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Height = 36
                };

                // Instantiate an Ellipse shape to represent a user icon with properties inside
                Ellipse userIcon = new Ellipse
                {
                    Width = 30,
                    Height = 29,
                    Fill = new SolidColorBrush(Colors.LightBlue),
                    Margin = new Thickness(0, 0, 10, 0)
                };

                // Instantiate StackPannel for the username location with properties inside
                StackPanel userNameLocationPanel = new StackPanel
                {
                    Width = 120
                };

                // Add contents and extra properties to user name location panel 
                userNameLocationPanel.Children.Add(new TextBlock
                {
                    Text = review.UserName,
                    FontWeight = FontWeights.Bold
                });

                // Add more contents and properties to user name location pannel
                userNameLocationPanel.Children.Add(new TextBlock
                {
                    Text = "Waterloo, ON",
                    FontStyle = FontStyles.Italic
                });

                // Add user icon object into user info panel
                userInfoPanel.Children.Add(userIcon);

                // Add username location panel object into user info panel
                userInfoPanel.Children.Add(userNameLocationPanel);

                // Instantiate Stack Panel for rating date panel with properties inside
                StackPanel ratingDatePanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                // Add more contents and properties into the rating date panel 
                ratingDatePanel.Children.Add(new TextBlock
                {
                    Text = $"{review.Rating.ToString("0")} / 5 stars",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.OrangeRed)
                });

                // Add more contents and properties into the rating date panel
                ratingDatePanel.Children.Add(new TextBlock
                {
                    Text = review.DateOfPosting.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    Margin = new Thickness(10, 0, 0, 0)
                });

                // Instantiate TextBlock for review text with contents and properties inside
                TextBlock reviewTextBlock = new TextBlock
                {
                    Text = review.Review,
                    TextWrapping = TextWrapping.Wrap,
                    MinWidth = 500,
                    MaxWidth = 700,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                Button deleteButton = new Button
                {
                    Content = "Delete Review",
                    Style = (Style)this.FindResource("DeleteButtonStyle"),
                    Width = 100,
                    Height = 30,
                    CommandParameter = review,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                deleteButton.Click += DeleteReviewButton_Click;

                // Building the card with different children components (userinfo + rating date + review text + delete button) => One review card
                reviewContent.Children.Add(userInfoPanel);
                reviewContent.Children.Add(ratingDatePanel);
                reviewContent.Children.Add(reviewTextBlock);
                reviewContent.Children.Add(deleteButton);

                reviewCard.Child = reviewContent;

                // Finally, Add the card to the StackPanel
                ReviewsStackPanel.Children.Add(reviewCard);
            }

            // Calculate for average park rating 
            float averageRating = 0;

            if (reviews.Count > 0)
            {
                averageRating = totalRating / reviews.Count;
            }

            this.Dispatcher.Invoke(() =>
            {
                ParkReviewsCountTextBlock.Text = $"{reviews.Count} reviews";

                // In here, we will update our average rating for each park
                UpdateAverageRating(averageRating);

            });
        }

        private void DisplayParkData()
        {
            this.Dispatcher.Invoke(() =>
            {
                parkData = _client.ReceiveOneParkDataResponse();
                ParkNameTextBlock.Text = parkData.parkName;

                ParkRatingTextBlock.Text = $"Rating: {_averageRating.ToString("0.0")} stars";
                ParkHoursTextBox.Text = parkData.parkHours;
                //ParkReviewsCountTextBlock.Text = $"{parkData.numberOfReviews} reviews";
                ParkAddressTextBlock.Text = parkData.parkAddress;

                // Dynamically generate star ratings based on the rounded-down rating
                double roundedRating = Math.Floor(_averageRating);
                List<BitmapImage> stars = new List<BitmapImage>();
                for (int i = 0; i < roundedRating; i++)
                {
                    stars.Add(new BitmapImage(new Uri("/Assets/Icons/star-rating-icon.png", UriKind.Relative)));
                }
                StarRatingControl.ItemsSource = stars;

            });
        }

        private void DisplayParkImage(string parkName)
        {
            this.Dispatcher.Invoke(() =>
            {
                BitmapImage parkImage = _client.ReceiveOneParkImageResponse();

                SetParkImage(parkImage);

                _parkImage = parkImage;
            });
        }

        // Helper methods

        private void UpdateAverageRating(float averageRating)
        {
            _averageRating = averageRating;
        }

        private void SetParkImage(BitmapImage image)
        {
            ParkImage.Source = image;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ParkEditViewPage parkEditView = new ParkEditViewPage(parkData, _parkImage);

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
            Button button = sender as Button;
            if (button != null)
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.primitives.buttonbase.commandparameter?view=windowsdesktop-8.0
                // Here we are trying to type case the CommandParameter back into review object type
                ParkReviewManager.ParkReviewData review = button.CommandParameter as ParkReviewManager.ParkReviewData;

                if (review != null)
                {
                    string parkName = _parkName;
                    string userInfo = review.UserName;
                    string ratingInfo = $"{review.Rating.ToString("0")}";
                    string dateInfo = review.DateOfPosting.ToString("MM/dd/yyyy hh:mm:ss tt");
                    string reviewText = review.Review;

                    Console.WriteLine($"Park Name: {parkName}, User Info: {userInfo}, Rating: {ratingInfo}, Date: {dateInfo}, Review: {reviewText}");

                    // Send request to delete the data 

                    ParkReviewManager.ParkReviewData parkReviewData = new ParkReviewManager.ParkReviewData
                    {
                        ParkName = parkName,
                        UserName = userInfo,
                        Rating = float.Parse(ratingInfo),
                        DateOfPosting = DateTime.Parse(dateInfo),
                        Review = reviewText,
                    };

                    _client.SendDeleteReviewRequest(parkReviewData);

                    string serverResponse = _client.ReceiveServerResponse();

                    MessageBox.Show(serverResponse);

                    if (serverResponse.Contains("Review deleted successfully."))
                    {
                        Window parentWindow = Window.GetWindow(this);
                        if (parentWindow != null)
                        {
                            parentWindow.Close();
                        }
                    }
                }
            }
        }

        private void DeleteParkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _client.SendDeleteAParkRequest(_parkName);

                string serverResponse = _client.ReceiveServerResponse();

                MessageBox.Show(serverResponse);

                if (serverResponse.Contains($"{_parkName} has been deleted -> (park data, park image, park reviews)"))
                {
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as network errors
                MessageBox.Show($"An error occurred while trying to delete the park: {ex.Message}", "Error");
            }
        }
    }
}
