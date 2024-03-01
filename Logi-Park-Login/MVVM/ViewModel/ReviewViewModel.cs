using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.ViewModel
{
    public class ReviewViewModel
    {
        public ObservableCollection<Review> Reviews { get; set; }

        public ReviewViewModel()
        {
            Reviews = new ObservableCollection<Review>
        {
            new Review
            {
                ReviewerName = "Tony Stark",
                ReviewDate = "Dec 24, 2023",
                Rating = 5,
                ReviewText = "I came to realize that I had more to offer this world than just making things that blow up..."
            },
        };
        }
    }

    public class Review
    {
        public string ReviewerName { get; set; }
        public string ReviewDate { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }

}
