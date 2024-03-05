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
    /// Interaction logic for ParkViewPage.xaml
    /// </summary>
    public partial class ParkViewPage : UserControl
    {
        private ParkDataManager.ParkData _parkData;

        public ParkDataManager.ParkData ParkData
        {
            get => _parkData;
            set
            {
                _parkData = value;
                LoadParkDetails(); 
            }
        }

        public ParkViewPage()
        {
            InitializeComponent();
        }


        private void LoadParkDetails()
        {
            if (_parkData != null)
            {
                ParkNameTextBlock.Text = _parkData.GetParkName();
                // need to add park description for this one to work
                //ParkDescriptionTextBlock.Text = _parkData.GetParkDescription();
                ParkReviewTextBlock.Text = $"Rating: {_parkData.GetParkReview()} / 5.0 ({_parkData.GetNumberOfReviews()} reviews)";

                // what to do for the image once that part is figured out
                // i believe the iamge manager would be called here once its ready
                // ParkImage.Source = new BitmapImage(new Uri("path/to/your/image"));

            }
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            // write a reveiw code goes here
            //WriteReview.Click(sender, e) = the constuctor for the review module;

        }
    }
}
