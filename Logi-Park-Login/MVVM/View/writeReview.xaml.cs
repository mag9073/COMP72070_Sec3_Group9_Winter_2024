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
    /// Interaction logic for writeReview.xaml
    /// </summary>
    public partial class writeReview : Page
    {
        public writeReview()
        {
            InitializeComponent();
        }

        private void PostReviewButton_Click(object sender, RoutedEventArgs e)
        {
            string logReview = userReview.Text;
            byte[] reviewBytes = Encoding.Default.GetBytes(logReview);
            Logger log = new Logger("../../../ClientLog.txt"); // assume thats the correct file path 
            log.Log(reviewBytes);
            this.NavigationService.Navigate(new ParkViewPage());
        }

        private void CancelReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ParkViewPage());
        }

    }
}
