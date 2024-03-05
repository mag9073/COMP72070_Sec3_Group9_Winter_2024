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
                SizeToContent = SizeToContent.WidthAndHeight
            };
            window.Show();  // Right now, im creating this
        }
        

    private void RequestAllParkData()
    {
        // send request to the server retrieve all the park data info - we send empty body with the header containing flag to get all park data
        client.SendAllParkDataRequest();
        //client.SendAllParkImagesRequest();

        // receive back the response from the server which contains array of park data obj
        ParkDataManager.ParkData[] parks = client.ReceiveAllParkDataResponse();
        //var images = client.RequestAndReceiveImages();

        // We make it annoynmous types which consists of name, address, review
        var parkCards = parks.Select(park =>
        {
            //var image = images.FirstOrDefault(img => img.FileName == park.GetParkName() + ".jpg"); // Assuming naming convention
            return new
            {
                Name = park.GetParkName(),
                Address = park.GetParkAddress(),
                Review = $"{park.GetParkReview()} stars",
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
                    MessageBox.Show($"Park Name: {parkName}");

                    ParkViewPage parkViewPage = new ParkViewPage(parkName);
                    Window window = new Window
                    {
                        Content = parkViewPage,
                        SizeToContent = SizeToContent.WidthAndHeight
                    };
                    window.Show();  // Right now, im creating this 
                }
            }
        }
    }
}
}
