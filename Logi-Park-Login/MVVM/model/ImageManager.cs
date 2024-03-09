using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LogiPark.MVVM.Model
{
    public class ImageManager
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public BitmapImage Image { get; set; }
    }
}
