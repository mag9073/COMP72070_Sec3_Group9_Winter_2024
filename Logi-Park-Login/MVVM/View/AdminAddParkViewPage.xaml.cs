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
    /// Interaction logic for AdminAddParkViewPage.xaml
    /// </summary>
    public partial class AdminAddParkViewPage : UserControl
    {
        private string selectedImagePath;
        ProgramClient client;

        public AdminAddParkViewPage()
        {
            this.client = new ProgramClient();
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(ParkNameTextBox.Text))
            {
                MessageBox.Show("Please fill out Park Name field.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(ParkAddressTextBox.Text))
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


            // Assemble our data into packet
            ParkDataManager.ParkData parkData = new ParkDataManager.ParkData
            {
                parkName = ParkNameTextBox.Text,
                parkAddress = ParkAddressTextBox.Text,
                parkDescription = ParkDescriptionsTextBox.Text,
                parkHours = ParkHoursTextBox.Text,
            };

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                client.SendAddAParkDataRequest(parkData, selectedImagePath);

                string serverResponse = client.ReceiveServerResponse();

                MessageBox.Show(serverResponse);

                if (serverResponse.Contains("Park data added successfully."))
                {
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                }

            }
            else
            {
                MessageBox.Show("Please upload a park image.", "Image Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //Window parentWindow = Window.GetWindow(this);
            //parentWindow?.Close();
        }

        private void UplaodButton_Click(object sender, RoutedEventArgs e)
        {
            // File Dialog Configs
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JPEG Files (*.jpg)|*.jpg";
            dialog.Title = "Select a JPG Image";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();


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
            bool isValid = false;

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
