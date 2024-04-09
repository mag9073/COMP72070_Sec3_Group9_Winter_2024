using LogiPark.MVVM.Model;
using LogiPark.MVVM.ViewModel;
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

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for ParkEditViewPage.xaml
    /// </summary>
    public partial class ParkEditViewPage : UserControl
    {
        ProgramClient client;
        private ParkDataManager.ParkData _parkData;
        private BitmapImage _parkImage;
        private string selectedImagePath;

        public ParkEditViewPage()
        {

        }

        public ParkEditViewPage(ParkDataManager.ParkData parkData, BitmapImage parkImage)
        {
            this.client = new ProgramClient();
            _parkData = parkData;
            _parkImage = parkImage;
            InitializeComponent();
            UpdateTextBoxFields();
            UpdateParkImageDisplay();
        }

        private void UpdateTextBoxFields()
        {
            ParkNameTextBox.Text = _parkData.parkName;
            ParkAddressTextBox.Text = _parkData.parkAddress;
            ParkDescriptionsTextBox.Text = _parkData.parkDescription;
            ParkHoursTextBox.Text = _parkData.parkHours;

            // Make it look like it is undeditable by the admin -> park name
            //https://stackoverflow.com/questions/979876/set-background-color-of-wpf-textbox-in-c-sharp-code
            // I want the textblock is be more custom so i have to dynamically do this here
            // https://stackoverflow.com/questions/15470684/binding-hex-value-to-color-in-xaml
            ParkNameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235)); // Light gray background
            ParkNameTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221)); // Light gray border
            ParkNameTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)); // Darker text color

            ParkNameTextBox.BorderThickness = new Thickness(1);
        }

        private void UpdateParkImageDisplay()
        {
            ParkImage.Source = _parkImage;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(ParkAddressTextBox.Text))
            {
                MessageBox.Show("Please fill out Park Address field.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(ParkDescriptionsTextBox.Text))
            {
                MessageBox.Show("Please fill out Park Descriptions field.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(ParkHoursTextBox.Text))
            {
                MessageBox.Show("Please fill out Park Hours field.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Assemble our data into object (not packet yet sorry)
            ParkDataManager.ParkData parkData = new ParkDataManager.ParkData
            {
                parkName = ParkNameTextBox.Text,
                parkAddress = ParkAddressTextBox.Text,
                parkDescription = ParkDescriptionsTextBox.Text,
                parkHours = ParkHoursTextBox.Text,
            };

            // If the selectedImagePath is not empty, then we know a new park image was uploaded -> then we will send the local path of the image 
            // Update outcomes -> parkdata + new parkimage
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                client.SendEditAParkDataRequest(parkData, selectedImagePath);

            }
            else
            {
                // Instead of having creating a new file from bitmapimage source that it got back from the server when the page was loaded
                // And since they didnt need to update the image -> no use having to send the exact image 
                // only do it if it was updated -> a bit more efficient this way -> not sending unnecessary data
                client.SendEditAParkDataRequest(parkData, null);
            }

            string serverResponse = client.ReceiveServerResponse();

            MessageBox.Show(serverResponse);

            // Close the current view page after the save button is clicked
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }

        }

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // File Dialog Configs
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JPEG Files (*.jpg)|*.jpg";
            dialog.Title = "Select a JPG Image";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Reuse add a park data -> add image implementation
            if (result == true)
            {
                if (ValidateFileExtension(dialog.FileName))
                {

                    // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-use-a-bitmapimage?view=netframeworkdesktop-4.8

                    ParkImage.Source = new BitmapImage(new Uri(dialog.FileName));
                    selectedImagePath = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("Only JPG files are allowed here.", "Invalid File Type", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateFileExtension(string fileName)
        {
            // Confirm whether the file type is really jpg extension
            string extension = System.IO.Path.GetExtension(fileName).ToLower();
            bool isValid;

            if (extension == ".jpg")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
